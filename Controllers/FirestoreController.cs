using DemoDPGJobData.Models;
using DemoDPGJobData.ViewModels;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class FirestoreController : Controller
{
    private readonly FirestoreDb _firestoreDb;

    public FirestoreController(FirestoreDb firestoreDb){
        _firestoreDb = firestoreDb;
    }

    /// <summary>
    /// View all the data in the Firestore database
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult>ViewAllData(){
        CollectionReference collection = _firestoreDb.Collection("DemoJobData");
        QuerySnapshot snapshot = await collection.GetSnapshotAsync();
        var documents = snapshot.Documents.Select(d => d.ConvertTo<DemoJobData>()).ToList();

        CollectionReference collection2 = _firestoreDb.Collection("JobOp");
        QuerySnapshot snapshot2 = await collection2.GetSnapshotAsync();
        var operations = snapshot2.Documents.Select(d => d.ConvertTo<JobOp>()).ToList();

        var result = from d in documents join o in operations on d.JobOpId equals o.Id
                        select new JobDataViewModel
                            {
                                DemoJobData = d,
                                JobOp = o
                            };
                            

        return View(result.ToList().AsEnumerable());   
    }

    /// <summary>
    /// Present the view for entering new Data
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> AddData(){
        CollectionReference collection = _firestoreDb.Collection("JobOp");
        QuerySnapshot snapshot = await collection.GetSnapshotAsync();
        var operations = snapshot.Documents.Select(d => d.ConvertTo<JobOp>()).ToList();
        ViewData["JobOps"] = new SelectList(operations, "Id", "OpName");
        

        return View();
    }

    /// <summary>
    /// Add the new data to the database collection named "DemoJobData"
    /// </summary>
    /// <param name="demoJobData"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult>AddData(JobDataViewModel model)
    {
        model.DemoJobData.JobDataId = Guid.NewGuid().ToString();

        DateTime utcDate = model.DemoJobData.DateToday.Date.ToUniversalTime();

        DateTime dateTime1 = DateTime.ParseExact(model.DemoJobData.StartTime, "HH:mm", null);
        DateTime dateTime2 = DateTime.ParseExact(model.DemoJobData.EndTime, "HH:mm", null);
        TimeSpan difference = dateTime2 - dateTime1;
        
        CollectionReference collection = _firestoreDb.Collection("DemoJobData");
        DocumentReference document = await collection.AddAsync(new DemoJobData
        {
            JobDataId = model.DemoJobData.JobDataId,
            Initials= model.DemoJobData.Initials,
            JobNum = model.DemoJobData.JobNum,
            StartTime = model.DemoJobData.StartTime,
            EndTime = model.DemoJobData.EndTime,
            Quantity = model.DemoJobData.Quantity,
            DateToday = utcDate,
            Minutes = difference.TotalMinutes,
            JobOp = model.JobOp,
            JobOpId = model.JobOp.Id
        });
        return RedirectToAction("AddData");
    }

    /// <summary>
    /// View all the data in the Firestore database
    /// </summary>
    /// <returns></returns>
    // public async Task<IActionResult>EditData(){
    //     CollectionReference collection = _firestoreDb.Collection("DemoJobData");
    //     QuerySnapshot snapshot = await collection.GetSnapshotAsync();
    //     var documents = snapshot.Documents.Select(d => d.ConvertTo<DemoJobData>()).ToList();

    //     CollectionReference collection2 = _firestoreDb.Collection("JobOp");
    //     QuerySnapshot snapshot2 = await collection2.GetSnapshotAsync();
    //     var operations = snapshot2.Documents.Select(d => d.ConvertTo<JobOp>()).ToList();

    //     var result = from d in documents join o in operations on d.JobOpId equals o.Id
    //                     select new JobDataViewModel
    //                         {
    //                             DemoJobData = d,
    //                             JobOp = o
    //                         };
                            

    //     return View(result.ToList().AsEnumerable());   
    // }

}
