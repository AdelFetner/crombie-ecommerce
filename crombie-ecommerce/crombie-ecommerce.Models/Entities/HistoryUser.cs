﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crombie_ecommerce.Models.Entities
{
    [Table("HistoryUsers")]
    public class HistoryUser : History
    {
        //Does not require any additional fields, data is stored in JSON format
    }
}
