using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleMonitor.Models
{
    public class Equipment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Area { get; set; }

        public string Reason { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public TimeSpan Duration { get; set; }

        public bool IsActive { get; set; }

        public static int InsertFault(Equipment equipment)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.Database))
            {
                conn.CreateTable<Equipment>();
                return conn.Insert(equipment);
            }
        }

        public static void DeactivateFault()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.Database))
            {
                conn.CreateTable<Equipment>();
                var activeFault = conn.Table<Equipment>().Where(u => u.IsActive == true).FirstOrDefault();
                activeFault.IsActive = false;

                conn.Update(activeFault);
            }
        }

        public static bool IsThereActiveFault()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.Database))
            {
                conn.CreateTable<Equipment>();
                var yesThereIs = conn.Table<Equipment>().Where(u => u.IsActive == true).Count() > 0;

                if (yesThereIs)
                    return true;
                else
                {
                    return false;
                }
            }
        }
    }
}
