using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace code_learning_spectacles_cli.Models
{
  internal class DeviceVerification
  {
    public string device_code { get; set; } = string.Empty;
    public string user_code { get; set; } = string.Empty;
    public string verification_uri { get; set; } = string.Empty;
    public int expires_in { get; set; }
    public int interval { get; set; }
  }
}
