namespace TekstilScada.UIControls
{
    /// <summary>
    /// Reçete adımlarını düzenleyen kullanıcı kontrolleri için ortak bir arayüz.
    /// Bu arayüz, her bir adım düzenleyicisinin mevcut adım verisini almasını (GetStep)
    /// ve bir adım verisiyle doldurulmasını (SetStep) sağlar. Bu, ProsesKontrol_Control içerisindeki
    /// mantığı basitleştirir ve yeni adım tipleri eklemeyi kolaylaştırır.
    /// </summary>
    public interface IStepEditor
    {
        /// <summary>
        /// Kullanıcı arayüzündeki girdilere göre bir ScadaRecipeStep nesnesi oluşturur ve döndürür.
        /// </summary>
        /// <returns>Doldurulmuş TekstilScada.Models.ScadaRecipeStep nesnesi.</returns>
        Models.ScadaRecipeStep GetStep();

        /// <summary>
        /// Verilen ScadaRecipeStep nesnesindeki verilerle kullanıcı arayüzünü doldurur.
        /// </summary>
        /// <param name="step">Arayüzü doldurmak için kullanılacak TekstilScada.Models.ScadaRecipeStep nesnesi.</param>
        void SetStep(Models.ScadaRecipeStep step);
    }
}
