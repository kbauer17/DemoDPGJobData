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
        return View(documents);   
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
        
        CollectionReference collection = _firestoreDb.Collection("DemoJobData");
        DocumentReference document = await collection.AddAsync(new DemoJobData
        {
            Initials= model.DemoJobData.Initials,
            JobNum = model.DemoJobData.JobNum,
            StartTime = model.DemoJobData.StartTime,
            EndTime = model.DemoJobData.EndTime,
            Quantity = model.DemoJobData.Quantity,
            JobOp = model.JobOp,
            JobOpId = model.JobOp.Id
        });
        return RedirectToAction("AddData");
    }


}
