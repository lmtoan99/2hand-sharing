using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    class DonatePostInformation : BaseEntity
    {
        private string Description;
        private DateTime PostTime;
        private bool Status;
        private int ItemId;
    }
}
