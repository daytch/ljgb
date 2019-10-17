using ljgb.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ljgb.DataAccess
{
    public class Class1
    {
        private static void Main(string[] args)
        {
            List<Warna> ListWarna = new List<Warna>();
            using (var ctx = new ljgbContext())
            {
               ListWarna = ctx.Warna.Where(x => x.RowStatus == true).ToList();
            }
            foreach (var item in ListWarna)
            {
                Console.WriteLine(item.Barang);
            }

            Console.ReadKey();
        }
    }
}
