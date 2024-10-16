using DemoDPGJobData.Models;
using DemoDPGJobData.ViewModels;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;

public class AdministrationController : Controller
{
    private readonly FirestoreDb _firestoreDb;
    private readonly ILogger<AdministrationController> _logger;

    public AdministrationController(FirestoreDb firestoreDb, ILogger<AdministrationController> logger){
        _firestoreDb = firestoreDb;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    
    /// <summary>
    /// View all the Operations in the Firestore database
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult>ViewAllOperations(){
        CollectionReference collection2 = _firestoreDb.Collection("JobOp");
        QuerySnapshot snapshot2 = await collection2.GetSnapshotAsync();
        var operations = snapshot2.Documents.Select(d => d.ConvertTo<JobOp>()).ToList();

        return View(operations);   
    }

    /// <summary>
    /// Create a new Operation in the Firestore database
    /// </summary>
    /// <returns></returns>
    public IActionResult AddOperation()
    {
        return View();
    }

    /// <summary>
    /// Add the new Operation to the database collection named "JobOp"
    /// </summary>
    /// <param name="demoJobData"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult>AddOperation(AdministrationViewModel model)
    {
        CollectionReference collection = _firestoreDb.Collection("JobOp");
        DocumentReference document = await collection.AddAsync(new JobOp
        {

            OpName = model.JobOp.OpName,
            Id = model.JobOp.Id
        });
        return RedirectToAction("AddOperation");
    }


    /// <summary>
    /// Edit an Operation in the Firestore database
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult>EditOperation(int id)
    {
        var collection = _firestoreDb.Collection("JobOp");
        var query = collection.WhereEqualTo("Id", id);
        var snapshot = await query.GetSnapshotAsync();
        
        if(snapshot.Documents.Any())
        {
            var document = snapshot.Documents[0];
            var jobOp = document.ConvertTo<JobOp>(); //ensure the model class matches the Firestore document structure

            _logger.LogInformation("Document found: {DocumentPath}", document.Reference.Path);

            var model = new AdministrationViewModel
            {
                JobOp = jobOp
            };
            return View(model);
        }

        _logger.LogError("No document found with Id: {Id}", id);
        return NotFound();   
    }

    /// <summary>
    /// Edit an operation in the database collection named "JobOp"
    /// </summary>
    /// <param name="demoJobData"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult>EditOperation(JobDataViewModel model)
    {
        var collection = _firestoreDb.Collection("JobOp");
        var query = collection.WhereEqualTo("Id", model.JobOp.Id);
        var snapshot = await query.GetSnapshotAsync();

        if(!snapshot.Documents.Any())
        {
            _logger.LogError("Document not found with Id: {Id}", model.JobOp.Id);
            return NotFound();
        }

        var documentReference = snapshot.Documents[0].Reference;


        var updateData = new Dictionary<string, object>
        {
            {"Id", model.JobOp.Id},
            {"OpName", model.JobOp.OpName}
        };

        await documentReference.SetAsync(updateData);

        return RedirectToAction("ViewAllOperations");
    }


}