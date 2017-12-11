using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace PrintScreen
{
    class Lerp
    {
        double valueToChange;
        double start;
        double end;
        double duration;
        double amount;
        int currentMillis = 0;
        double amountPerTick = 0;
        System.Timers.Timer tickTimer;
        Form form;

        public Lerp(double valueToChange,double start,double end,double duration)
        {
            

            this.valueToChange = valueToChange;
            this.start = start;
            this.end = end;
            this.duration = duration;

            double range = end-start;
            amountPerTick = range / duration;

            

            Console.Out.WriteLine("AmountPerTick: " + amountPerTick);
            valueToChange = start;
        }

        public void SetForm(Form form)
        {
            this.form = form;
            //FadeIn(form, 100);
            FadeOut(form, 1,1000);
        }

        public void Start()
        {
            tickTimer = new System.Timers.Timer();
            tickTimer.Interval = 1;
            tickTimer.Elapsed += TickTimer_Elapsed;
            tickTimer.Start();
        }

        private async void FadeIn(Form o, int interval = 80)
        {
            //Object is not fully invisible. Fade it in
            while (o.Opacity < 1.0)
            {
                await Task.Delay(interval);
                o.Opacity += 0.05;
            }
            o.Opacity = 1; //make fully visible       
        }

        private async void FadeOut(Form o, int interval,int duration)
        {
            await Task.Delay(duration);
            //Object is fully visible. Fade it out
            while (o.Opacity > 0.0)
            {
                await Task.Delay(interval);
                //Thread.Sleep(1);
                o.Opacity -= 0.01;
            }
            o.Opacity = 0; //make fully invisible    
            o.Hide();
            o.Dispose();
        }

        private void TickTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            currentMillis+=10;
            if (currentMillis > duration)
            {
                tickTimer.Stop();
                tickTimer.Dispose();
            }

            //form.Opacity = valueToChange;
            //Console.Out.WriteLine("valueToChange: " + valueToChange);
        }
    }
}
