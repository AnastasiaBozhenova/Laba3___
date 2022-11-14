namespace Lab3_.ViewModels
{
    public enum SortState
    {
        No,
        NameAsc,
        NameDesc,
        MeasurementAsc,
        MeasurementDesc
    }

    public class SortViewModel
    {
        public SortViewModel(SortState sortOrder)
        {
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            MeasurementSort = sortOrder == SortState.MeasurementAsc ? SortState.MeasurementDesc : SortState.MeasurementAsc;
            CurrentState = sortOrder;
        }

        public SortState NameSort { get; set; }
        public SortState MeasurementSort { get; set; }
        public SortState CurrentState { get; set; }
    }
}