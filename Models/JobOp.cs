using Google.Cloud.Firestore;

namespace DemoDPGJobData.Models;

[FirestoreData]
public class JobOp
{
    public JobOp(){}

    [FirestoreProperty]
    public required int Id {get; set;}

    [FirestoreProperty]
    public required string OpName {get; set;}
}