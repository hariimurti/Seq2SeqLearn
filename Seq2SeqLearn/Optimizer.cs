using System;
using System.Collections.Generic;

namespace Seq2SeqLearn
{
    public class Optimizer
    {
        private double decay_rate = 0.999;
        private double smooth_eps = 1e-8;
        private List<WeightMatrix> step_cache = new List<WeightMatrix>();

        public void setp(List<WeightMatrix> model, double step_size, double regc, double clipval)
        {
            var num_clipped = 0;
            var num_tot = 0;
            foreach (var k in model)
            {
                if (k == null)
                {
                    continue;
                }
                var m = k; // mat ref
                var s = k.Cash;
                for (int i = 0, n = m.Weight.Length; i < n; i++)
                {
                    // rmsprop adaptive learning rate
                    var mdwi = m.Gradient[i];
                    s[i] = s[i] * this.decay_rate + (1.0 - this.decay_rate)
                        * mdwi * mdwi;

                    // gradient clip
                    if (mdwi > clipval)
                    {
                        mdwi = clipval;
                        num_clipped++;
                    }
                    if (mdwi < -clipval)
                    {
                        mdwi = -clipval;
                        num_clipped++;
                    }
                    num_tot++;

                    // update (and regularize)
                    m.Weight[i] += -step_size *
                        mdwi / Math.Sqrt(s[i] + this.smooth_eps) -
                        regc * m.Weight[i];
                    m.Gradient[i] = 0; // reset gradients for next iteration
                }
            }
        }
    }
}