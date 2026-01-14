using System.Collections.Generic;

namespace YG
{
    public partial class SavesYG
    {
        public int dollarsCount = 0; 
        public int failsCount = 0;
        public int chelixCount = 0;
        public int bronzeCount = 0;
        public int siverCount = 0;
        public int goldCount = 0;
        public List<SaveData> itemsData;
        public List<SaveUpgradeData> itemsUpdateData;

        public void SetDefault()
        {
            dollarsCount = 0; 
            failsCount = 0;
            chelixCount = 0;
            bronzeCount = 0;
            siverCount = 0;
            goldCount = 0;
            itemsData = new List<SaveData>();
            itemsUpdateData = new List<SaveUpgradeData>();
        }
    }
}
