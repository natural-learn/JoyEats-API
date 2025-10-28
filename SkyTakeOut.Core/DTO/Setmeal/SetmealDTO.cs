using SkyTakeOut.Models;

namespace SkyTakeOut.Core.DTO.Setmeal
{
    public class SetmealDTO
    {
        public long Id { get; set; }

        /// <summary>
        /// 分类Id
        /// </summary>
        public long? CategoryId { get; set; }

        /// <summary>
        /// 套餐名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 套餐价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 状态 0:停用 1:启用
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 套餐菜品关系
        /// </summary>
        public List<SetmealDish> SetmealDishes { get; set; } = new List<SetmealDish>();
    }
}
