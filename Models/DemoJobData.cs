using Google.Cloud.Firestore;

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

}