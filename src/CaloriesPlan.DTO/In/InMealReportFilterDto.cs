using System;
using System.ComponentModel.DataAnnotations;

namespace CaloriesPlan.DTO.In
{
    public class InMealReportFilterDto
    {
        private DateTime? dateFrom;
        private DateTime? dateTo;
        private DateTime? timeFrom;
        private DateTime? timeTo;

        private int pageSize = 10;
        private int page = 0;

        public DateTime? DateFrom
        {
            get
            {
                if (this.dateFrom != null)
                    return this.dateFrom.Value.ToUniversalTime();

                return this.dateFrom;
            }
            set { this.dateFrom = value; }
        }

        public DateTime? DateTo
        {
            get
            {
                if (this.dateTo != null)
                    return this.dateTo.Value.ToUniversalTime();

                return this.dateTo;
            }
            set { this.dateTo = value; }
        }

        public DateTime? TimeFrom
        {
            get
            {
                if (this.timeFrom != null)
                    return this.timeFrom.Value.ToUniversalTime();

                return this.timeFrom;
            }
            set { this.timeFrom = value; }
        }

        public DateTime? TimeTo
        {
            get
            {
                if (this.timeTo != null)
                    return this.timeTo.Value.ToUniversalTime();

                return this.timeTo;
            }
            set { this.timeTo = value; }
        }

        [Range(1, int.MaxValue, ErrorMessage = "Value should be more than 0.")]
        public int PageSize
        {
            get { return this.pageSize; }
            set { this.pageSize = value; }
        }

        [Range(0, int.MaxValue, ErrorMessage = "Value should be a positive integer.")]
        public int Page
        {
            get { return this.page; }
            set { this.page = value; }
        }
    }
}
