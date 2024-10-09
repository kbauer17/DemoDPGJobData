using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;

namespace DemoDPGJobData.Models;

[FirestoreData]
public class DemoJobData
{
    public DemoJobData(){}

    [FirestoreProperty]
    public required string Initials {get; set;}

    [FirestoreProperty]
    public required string JobNum {get; set;}
    
    [FirestoreProperty]
    public required string StartTime {get; set;}

    [FirestoreProperty]
    public required string EndTime {get; set;}

    [FirestoreProperty]
    public required int Quantity{get; set;}

    [BindProperty]
    [FirestoreProperty]
    public required int JobOpId {get; set;}

    public required virtual JobOp JobOp {get; set;}

   
  

}