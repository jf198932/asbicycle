using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ASBicycle.Entities
{
    /// <summary>
    /// 故障反馈
    /// </summary>
    [Table("Tb_trouble_feedback")]
    public class Tb_trouble_feedback : Entity
    {
        public DateTime? create_time { get; set; }
        public DateTime? update_time { get; set; }
        /// <summary>
        /// 反馈用户
        /// </summary>
        public int? create_by { get; set; }
        /// <summary>
        /// 最后修改人
        /// </summary>
        public int? update_by { get; set; }
        /// <summary>
        /// 车辆损坏(坐垫损坏、链条损坏、踏脚损坏、龙头损坏、轮胎损坏、其他损坏)
        /// </summary>
        public string trouble1 { get; set; }
        /// <summary>
        /// 用车故障(车锁故障、还车故障)
        /// </summary>
        public string trouble2 { get; set; }
        /// <summary>
        /// 违规用车(举报违停、上私锁、不锁车、私卸车牌、而已损坏、疑似偷车)
        /// </summary>
        public string trouble3 { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string comments { get; set; }
        /// <summary>
        /// 照片地址
        /// </summary>
        public string img_url { get; set; }
        /// <summary>
        /// 核实情况，1.待核实，2.已经核实，3.非属实(虚报)
        /// </summary>
        public int? verify_status { get; set; }
        /// <summary>
        /// 客服处理情况, 1.用户提交, 2.客服处理中，3.客服已经处理
        /// </summary>
        public int? deal_status { get; set; }
    }
}