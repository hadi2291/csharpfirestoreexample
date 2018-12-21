using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Text;

namespace bancocomposicao
{

    [FirestoreData]
    public class FileId
    {
        [FirestoreProperty]
        public string Id { get; set; }
    }
    [FirestoreData]
    public class BenchCompositionItemAnalytical
    {
        [FirestoreProperty]
        public string Type { get; set; }
        [FirestoreProperty]
        public string Reference { get; set; }
        [FirestoreProperty]
        public string Code { get; set; }
        [FirestoreProperty]
        public string Description { get; set; }
        [FirestoreProperty]
        public double Coefficient { get; set; }
        [FirestoreProperty]
        public double UnityCost { get; set; }
        [FirestoreProperty]
        public double TotalCost { get; set; }


    }

    //TODO - FirestoreID = IDFile + code
    [FirestoreData]
    public class BenchCompositionItemSynthetic
    {
        [FirestoreProperty]
        public FileId File { get; set; } // id level
        [FirestoreProperty]
        public Dictionary<string, string> Level { get; set; } // id level
        [FirestoreProperty]
        public string Code { get; set; }
        [FirestoreProperty]
        public string Description { get; set; }
        [FirestoreProperty]
        public string Type { get; set; }
        [FirestoreProperty]
        public string Unity { get; set; }
        [FirestoreProperty]
        public string PriceRise { get; set; }
        [FirestoreProperty]
        public double TotalCost { get; set; }
        [FirestoreProperty]
        public string Link { get; set; }
        [FirestoreProperty]
        public double LaborCost { get; set; }
        [FirestoreProperty]
        public double LaborCostPercenage{ get; set; }
        [FirestoreProperty]
        public double MaterialCost{ get; set; }
        [FirestoreProperty]
        public double MaterialCostPercentage { get; set; }
        [FirestoreProperty]
        public double EquipmentCost { get; set; }
        [FirestoreProperty]
        public double EquipmentCostPecentage { get; set; }
        [FirestoreProperty]
        public double ThirdPartyServiceCost { get; set; }
        [FirestoreProperty]
        public double ThirdPartyServiceCostPercentage { get; set; }
        [FirestoreProperty]
        public double OtherCost { get; set; }
        [FirestoreProperty]
        public double OtherCostPercentage { get; set; }
        [FirestoreProperty]
        public BenchCompositionItemAnalytical[] Composition { get; set; } // id ItemAnalytical



    }
}
