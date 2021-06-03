using System.Linq;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;

    public class ImageGeneration
    {
        private string filePath;
        public void SetFile(string file)
        {
            this.filePath = file;
        }

        public void Generate(List<Product> prods)
        {
            var plt = new ScottPlot.Plot(600, 400);

            
            double[] values = CreateListPrice(prods).Keys.ToArray<double>();
            double[] price = CreateListPrice(prods).Values.ToArray<double>();
            var hist = new ScottPlot.Statistics.Histogram(values, min: 1, max: prods.Count);


            double barWidth = hist.binSize * 1.2;
            plt.PlotBar(values, price, barWidth: barWidth, outlineWidth: 0);
            plt.Title("Distribution of prices for products");
            
            plt.YLabel("Prices");
            plt.XLabel("Products");

            plt.SaveFig(this.filePath);
        }
        public Dictionary<double,double> CreateListPrice(List<Product> list)
        {
            Dictionary<double,double> listprice = new Dictionary<double,double>();
            int i = 1;
            foreach(Product item in list)
            {
                listprice.Add(i, item.price);
                i++;
            }
            return listprice;
        }
    }

