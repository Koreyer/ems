using AutoMapper;
using EMS.Models;
using EMS02.EntityModels.EM02.LoginBusiness;
using EMS05.ModelsDTO.EMD01.SystemDTO;
using EMS05.ModelsDTO.EMD02.LoginBusinessDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMS06.DataService.MapperTools
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            #region 用户转换
            CreateMap<User, UserDTO>().ForMember(x => x.SexName, y => y.MapFrom(a => a.Sex));
            CreateMap<UserDTO, User>().ForMember(x => x.Sex, y => y.MapFrom(a => a.SexName));
            #endregion

            

            #region  设备使用申请表
            CreateMap<EquipmentUseWithUser, EquipmentUseWithUserDTO>().ForMember(x => x.ReturnSituationName, y => y.MapFrom(a => a.ReturnSituation)).ForMember(x => x.ExamineEnumName, y => y.MapFrom(a => a.ExamineEnum));
            CreateMap<EquipmentUseWithUserDTO, EquipmentUseWithUser>().ForMember(x => x.ReturnSituation, y => y.MapFrom(a => a.ReturnSituationName)).ForMember(x => x.ExamineEnum, y => y.MapFrom(a => a.ExamineEnumName));
            #endregion

            #region 账户转换
            CreateMap<Login, LoginDTO>();
            CreateMap<LoginDTO, Login>();
            #endregion

            #region 设备转换
            CreateMap<Equipment, EquipmentDTO>().ForMember(x=>x.SituationEnumName,y=>y.MapFrom(a=>a.SituationEnum)); 
            CreateMap<EquipmentDTO, Equipment>().ForMember(x => x.SituationEnum, y => y.MapFrom(a => a.SituationEnumName));
            #endregion
            #region 设备类别
            CreateMap<EquipmentCategory, EquipmentCategoryDTO>();
            CreateMap<EquipmentCategoryDTO, EquipmentCategory>();
            #endregion

            #region 设备报修表 
            CreateMap<EquipmentReport, EquipmentReportDTO>();
            CreateMap<EquipmentReportDTO, EquipmentReport>();
            #endregion

            #region  设备报废表
            CreateMap<EquipmentScrap, EquipmentScrapDTO>();
            CreateMap<EquipmentScrapDTO, EquipmentScrap>(); 
            #endregion

            #region  事务通报
            CreateMap<TransactionNotification, TransactionNotificationDTO>();
            CreateMap<TransactionNotificationDTO, TransactionNotification>();
            #endregion
        }
    }
}
