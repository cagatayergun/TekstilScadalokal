// Core/RecipeCsvConverter.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TekstilScada.Models;

namespace TekstilScada.Core
{
    public static class RecipeCsvConverter
    {
        /// <summary>
        /// Bir ScadaRecipe nesnesini HMI'ın anlayacağı CSV formatına dönüştürür.
        /// </summary>
        public static string ToCsv(ScadaRecipe recipe)
        {
            if (recipe == null || recipe.Steps == null) return "";

            var csvBuilder = new StringBuilder();

            // ÖNEMLİ: Buradaki mantık, HMI'daki CSV dosyasının formatına
            // birebir uymalıdır. Bu bir varsayımdır.
            // Örnek: Her satır bir adımı, her sütun bir Word'ü temsil eder.

            // Başlık satırı (opsiyonel, HMI formatına bağlı)
            // csvBuilder.AppendLine("Step,Word0,Word1,...,Word24");

            foreach (var step in recipe.Steps)
            {
                // StepDataWords dizisindeki 25 short değeri virgülle ayırarak birleştir
                string line = string.Join(",", step.StepDataWords.Select(w => w.ToString()));
                csvBuilder.AppendLine(line);
            }

            return csvBuilder.ToString();
        }

        /// <summary>
        /// Bir CSV dosyasının içeriğini ScadaRecipe nesnesine dönüştürür.
        /// </summary>
        public static ScadaRecipe ToRecipe(string csvContent, string recipeName)
        {
            var recipe = new ScadaRecipe { RecipeName = recipeName };
            if (string.IsNullOrWhiteSpace(csvContent)) return recipe;

            var lines = csvContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                  .ToList();

            // Eğer dosya başlık içeriyorsa (XPR00001.csv formatı gibi), bu başlıkları atla
            // Örnek: "Date", "Title", "Item Count" gibi başlıklar varsa atlanır.
            int dataStartIndex = 0;
            if (lines.Count > 3 && lines[0].StartsWith("Date") && lines[2].StartsWith("Item Count"))
            {
                dataStartIndex = 3;
            }

            // Sadece sayısal veri içeren satırları al
            var numericLines = lines.Skip(dataStartIndex)
                                    .Where(line => short.TryParse(line, out _))
                                    .ToList();

            int stepNumber = 1;
            // Verileri 25'erli gruplara ayırarak adımları oluştur
            for (int i = 0; i < numericLines.Count; i += 25)
            {
                // Grubun 25 eleman içerdiğinden emin ol
                var stepValues = numericLines.Skip(i).Take(25).ToList();
                if (stepValues.Count == 25)
                {
                    try
                    {
                        var step = new ScadaRecipeStep { StepNumber = stepNumber++ };
                        for (int j = 0; j < 25; j++)
                        {
                            step.StepDataWords[j] = Convert.ToInt16(stepValues[j]);
                        }
                        recipe.Steps.Add(step);
                    }
                    catch
                    {
                        // Hatalı satırları logla veya yoksay
                    }
                }
            }

            // Eğer yukarıdaki mantıkla veri bulunamazsa, orijinal virgülle ayrılmış formatı dene
         

            return recipe;
        }
    }
}
