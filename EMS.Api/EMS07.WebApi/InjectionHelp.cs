using DataRepository;
using EMS.Models;
using EMS02.EntityModels.EM02.LoginBusiness;
using EMS04.DataRepository;
using EMS05.ModelsDTO.EMD01.SystemDTO;
using EMS05.ModelsDTO.EMD02.LoginBusinessDTO;
using EMS06.DataService;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EMS07.WebApi
{
    public static class InjectionHelp
    {
        public static void Injection(this IServiceCollection services)
        {
            #region 登录
            //services.AddScoped<ILoginRepository, LoginRepository>();
            //services.AddScoped<ILoginApiService, LoginApiService>();
            services.AddScoped<IApiEntityService<Guid, Login, LoginDTO>, ApiEntityService<Guid, Login, LoginDTO>>();
            services.AddScoped<IRepository<Guid, Login>, Repository<Guid, Login>>();
            #endregion

            #region 事务通报
            services.AddScoped<IApiEntityService<Guid, TransactionNotification, TransactionNotificationDTO>, ApiEntityService<Guid, TransactionNotification, TransactionNotificationDTO>>();
            services.AddScoped<IRepository<Guid, TransactionNotification>, Repository<Guid, TransactionNotification>>();
            #endregion
            #region 设备类型
            services.AddScoped<IApiEntityService<Guid, EquipmentCategory, EquipmentCategoryDTO>, ApiEntityService<Guid, EquipmentCategory, EquipmentCategoryDTO>>();
            services.AddScoped<IRepository<Guid, EquipmentCategory>, Repository<Guid, EquipmentCategory>>();
            #endregion
            #region 设备信息
            services.AddScoped<IApiEntityService<Guid, Equipment, EquipmentDTO>, ApiEntityService<Guid, Equipment, EquipmentDTO>>();
            services.AddScoped<IRepository<Guid, Equipment>, Repository<Guid, Equipment>>();
            #endregion

            #region 设备维修信息
            services.AddScoped<IApiEntityService<Guid, EquipmentReport, EquipmentReportDTO>, ApiEntityService<Guid, EquipmentReport, EquipmentReportDTO>>();
            services.AddScoped<IRepository<Guid, EquipmentReport>, Repository<Guid, EquipmentReport>>();
            #endregion
            #region 设备申请
            services.AddScoped<IApiEntityService<Guid, EquipmentUseWithUser, EquipmentUseWithUserDTO>, ApiEntityService<Guid, EquipmentUseWithUser, EquipmentUseWithUserDTO>>();
            services.AddScoped<IRepository<Guid, EquipmentUseWithUser>, Repository<Guid, EquipmentUseWithUser>>();
            #endregion

            #region 设备报废
            services.AddScoped<IApiEntityService<Guid, EquipmentScrap, EquipmentScrapDTO>, ApiEntityService<Guid, EquipmentScrap, EquipmentScrapDTO>>();
            services.AddScoped<IRepository<Guid, EquipmentScrap>, Repository<Guid, EquipmentScrap>>();
            #endregion

        }
    }
}
