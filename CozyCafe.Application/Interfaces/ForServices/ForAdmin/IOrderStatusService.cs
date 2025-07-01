using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Models.DTO.Admin;

namespace CozyCafe.Application.Interfaces.ForServices.ForAdmin
{
    public interface IOrderStatusService
    {
        Task<bool> UpdateOrderStatusAsync(UpdateOrderStatusDto dto);
    }
}
