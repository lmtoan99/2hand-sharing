using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs.Firebase
{
    public class FirebaseRequest
    {
        [Required]
        public string FirebaseToken { get; set; }
    }
}
