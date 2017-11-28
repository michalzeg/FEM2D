using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class BeamProperties
    {
        public double ModulusOfElasticity { get; set; }
        public double MomentOfInertia { get; set; }
        public double Area { get; set; }


        public static BeamProperties Default => new BeamProperties
        {
            ModulusOfElasticity = 200,
            MomentOfInertia = 200,
            Area = 300
        };
    }
}
