namespace IAutor.Api.Data.Entities
{
    public sealed class Plan:Base
    {
        public string Title { get; set; }

        public decimal Price { get; set; }

        public string Currency { get; set; }

        public int MaxLimitSendDataIA { get; set; }

        public DateTime InitialValidityPeriod { get; set; }

        public DateTime? FinalValidityPeriod { get; set; }

        #region Methods
        public Plan() { }   
        #endregion
    }
}
