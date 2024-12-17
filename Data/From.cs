using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunamoMail.Data;

public record struct From(string Name, string Mail, string Password)
{
}
