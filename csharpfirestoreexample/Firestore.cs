using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;

namespace bancocomposicao
{
    public class Firestore
    {
        public FirestoreDb db { get; private set; }

        public Firestore()
        {
            db = FirestoreDb.Create("mega-rubk-dev");
        }


        public async Task ListDevelopments()
        {
            CollectionReference collection = db.Collection("developments");
            Console.WriteLine($"coll: {collection.Id}");
            QuerySnapshot querySnapshot = await collection.GetSnapshotAsync();
            Console.WriteLine($"coll.count: {querySnapshot.Count}");
            string name, status;
            DateTime createdAt;
            foreach (DocumentSnapshot queryResult in querySnapshot.Documents)
            {
                name = ""; status = ""; createdAt = new DateTime();
                if (queryResult.ContainsField("name"))
                    name = queryResult.GetValue<string>("name");
                if (queryResult.ContainsField("createdAt"))
                    createdAt = queryResult.GetValue<DateTime>("createdAt");
                if (queryResult.ContainsField("status"))
                    status = queryResult.GetValue<string>("status");
                Console.WriteLine($"{name} - {createdAt} - {status}");
            }

        }

        public async Task Listening1()
        {
            DocumentReference doc = db.Collection("testecsharp").Document();

            FirestoreChangeListener listener = doc.Listen(snapshot =>
            {
                Console.WriteLine($"Callback received document snapshot");
                Console.WriteLine($"Document exists? {snapshot.Exists}");
                if (snapshot.Exists)
                {
                    Console.WriteLine($"docs: {snapshot.GetValue<string>("name")} {snapshot.GetValue<string>("lastname")} - {snapshot.GetValue<int?>("age")}");
                }
                Console.WriteLine();
            });

            Console.WriteLine("Creating document");
            await doc.CreateAsync(new { name = "name", lastname="one", age = 10 });
            await Task.Delay(1000);

            Console.WriteLine($"Updating document");
            await doc.SetAsync(new { name = "name", lastname = "one point one", age = 11 });
            await Task.Delay(1000);

            Console.WriteLine($"Deleting document");
            await doc.DeleteAsync();
            await Task.Delay(1000);

            Console.WriteLine("Creating document again");
            await doc.CreateAsync(new { name = "name", lastname = "one", age = 10 });
            await Task.Delay(1000);

            Console.WriteLine("Stopping the listener");
            await listener.StopAsync();

            Console.WriteLine($"Updating document (no output expected)");
            await doc.SetAsync(new { name = "name", lastname = "two", age = 20 });
            await Task.Delay(1000);
        }

        public async Task Listening2()
        {
            CollectionReference collection = db.Collection("testecsharp");
            Query query = collection.WhereGreaterThan("age", 5).OrderByDescending("age");

            FirestoreChangeListener listener = collection.Listen(snapshot =>
            {
                Console.WriteLine($"Callback received query snapshot");
                Console.WriteLine($"Count: {snapshot.Count}");
                Console.WriteLine("Changes:");
                string name = ""; int age = 0;
                foreach (DocumentChange change in snapshot.Changes)
                {
                    DocumentSnapshot document = change.Document;
                    Console.WriteLine($"{document.Reference.Id}: ChangeType={change.ChangeType}; OldIndex={change.OldIndex}; NewIndex={change.NewIndex})");
                    if (document.Exists)
                    {
                        if (document.ContainsField("name"))
                            name = document.GetValue<string>("name");
                        if (document.ContainsField("age"))
                            age = document.GetValue<int>("age");
                        Console.WriteLine($"  Document data: Name={name}; age={age}");
                    }
                }
                Console.WriteLine();
            });

            Console.WriteLine("Creating document for Sophie (age = 7)");
            DocumentReference doc1Ref = await collection.AddAsync(new { name = "Sophie", age = 7 });
            Console.WriteLine($"Sophie document ID: {doc1Ref.Id}");
            await Task.Delay(1000);

            Console.WriteLine("Creating document for James (age = 10)");
            DocumentReference doc2Ref = await collection.AddAsync(new { name = "James", age = 10 });
            Console.WriteLine($"James document ID: {doc2Ref.Id}");
            await Task.Delay(1000);

            Console.WriteLine("Modifying document for Sophie (set age = 11, higher than age for James)");
            await doc1Ref.UpdateAsync("age", 11);
            await Task.Delay(1000);

            Console.WriteLine("Modifying document for Sophie (set age = 12, no change in position)");
            await doc1Ref.UpdateAsync("age", 12);
            await Task.Delay(1000);

            Console.WriteLine("Modifying document for James (set age = 4, below threshold for query)");
            await doc2Ref.UpdateAsync("age", 4);
            await Task.Delay(1000);

            Console.WriteLine("Deleting document for Sophie");
            await doc1Ref.DeleteAsync();
            await Task.Delay(1000);

            Console.WriteLine("Stopping listener");
            await listener.StopAsync();
        }

        public async Task SetCollection(string collectionName, string id, object data)
        {
            try
            {
                //Console.WriteLine($"    try add doc {id} with {data}");
                CollectionReference collection = db.Collection(collectionName);
                var doc = collection.Document(id);
                //Console.WriteLine(doc);
                if (doc != null)
                {
                    await doc.DeleteAsync();
                }
                var doc1Ref = await doc.CreateAsync(data);
                Console.WriteLine("Ok");
            } catch (Exception ex)
            {
                Console.Write($"erro arq: {id} - {ex.Message}");
                Console.WriteLine();
            }
        }

    }
}
