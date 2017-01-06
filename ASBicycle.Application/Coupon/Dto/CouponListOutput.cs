using System.Collections.Generic;
using Abp.Application.Services.Dto;
using ASBicycle.Bike.Dto;
using Castle.Components.DictionaryAdapter;

namespace ASBicycle.Coupon.Dto
{
    public class CouponListOutput : IOutputDto
    {
        public CouponListOutput()
        {
            Canuse = new EditableList<CouponDto>();
            Overdue = new EditableList<CouponDto>();
            Beused = new EditableList<CouponDto>();
        }
        /// <summary>
        /// 可使用
        /// </summary>
        public List<CouponDto> Canuse { get; set; }
        /// <summary>
        /// 过期的
        /// </summary>
        public List<CouponDto> Overdue { get; set; }
        /// <summary>
        /// 已使用
        /// </summary>
        public List<CouponDto> Beused { get; set; } 
    }
}