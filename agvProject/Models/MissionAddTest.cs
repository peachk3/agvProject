using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agvProject.Models
{
    public class MissionAddTest
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Num { get; set; }

        [NotMapped]            
        public int RowNo { get; set; }  // 표에서만 쓰는 자동 순번

    }
}
