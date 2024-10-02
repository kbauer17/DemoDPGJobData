using DemoDPGJobData.Models;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

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
    public IActionResult AddData(){
        return View();
    }

    /// <summary>
    /// Add the new data to the database collection named "DemoJobData"
    /// </summary>
    /// <param name="demoJobData"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult>AddData(DemoJobData demoJobData)
    {
        CollectionReference collection = _firestoreDb.Collection("DemoJobData");
        DocumentReference document = await collection.AddAsync(new DemoJobData
        {
            Initials= demoJobData.Initials,
            JobNum = demoJobData.JobNum,
            StartTime = demoJobData.StartTime,
            EndTime = demoJobData.EndTime
        });
        return RedirectToAction("AddData");
    }


}
