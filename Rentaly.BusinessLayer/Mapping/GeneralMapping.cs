using AutoMapper;
using Rentaly.DTOLayer.BranchDTOs;
using Rentaly.DTOLayer.BrandDTOs;
using Rentaly.DTOLayer.CarDTOs;
using Rentaly.DTOLayer.CarImageDTOs;
using Rentaly.DTOLayer.CarModelDTOs;
using Rentaly.DTOLayer.CategoryDTOs;
using Rentaly.DTOLayer.CustomerDTOs;
using Rentaly.DTOLayer.RentalDTOs;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.BusinessLayer.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<Branch, CreateBranchDTO>().ReverseMap();
            CreateMap<Branch, ResultBranchDTO>().ReverseMap();
            CreateMap<Branch, UpdateBranchDTO>().ReverseMap();

            CreateMap<Brand, CreateBrandDTO>().ReverseMap();
            CreateMap<Brand, ResultBrandDTO>().ReverseMap();
            CreateMap<Brand, UpdateBrandDTO>().ReverseMap();

            CreateMap<Car, CreateCarDTO>().ReverseMap();
            CreateMap<Car, ResultCarDTO>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.BrandName : null))
                .ForMember(dest => dest.CarModelName, opt => opt.MapFrom(src => src.CarModel != null ? src.CarModel.ModelName : null))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null))
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch != null ? src.Branch.BranchName : null))
                .ReverseMap();

            CreateMap<Car, UpdateCarDTO>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.BrandName : null))
                .ForMember(dest => dest.CarModelName, opt => opt.MapFrom(src => src.CarModel != null ? src.CarModel.ModelName : null))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null))
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch != null ? src.Branch.BranchName : null))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Branch != null ? src.Branch.City : null)) 
                .ForMember(dest => dest.CarImages, opt => opt.MapFrom(src => src.CarImages))
                .ReverseMap()
                .ForMember(dest => dest.Brand, opt => opt.Ignore())
                .ForMember(dest => dest.CarModel, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore()) 
                .ForMember(dest => dest.Branch, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CarImages, opt => opt.Ignore());

            CreateMap<CarImage, CreateCarImageDTO>().ReverseMap();
            CreateMap<CarImage, ResultCarImageDTO>().ReverseMap();
            CreateMap<CarImage, UpdateCarImageDTO>().ReverseMap();

            CreateMap<CarModel, CreateCarModelDTO>().ReverseMap();
            CreateMap<CarModel, ResultCarModelDTO>().ReverseMap();
            CreateMap<CarModel, UpdateCarModelDTO>().ReverseMap();

            CreateMap<Category, CreateCategoryDTO>().ReverseMap();
            CreateMap<Category, ResultCategoryDTO>().ReverseMap();
            CreateMap<Category, UpdateCategoryDTO>().ReverseMap();

            CreateMap<Customer, CreateCustomerDTO>().ReverseMap();
            CreateMap<Customer, ResultCustomerDTO>().ReverseMap();
            CreateMap<Customer, UpdateCustomerDTO>().ReverseMap();

            CreateMap<Rental, CreateRentalDTO>().ReverseMap();
            CreateMap<Rental, ResultRentalDTO>().ReverseMap();
            CreateMap<Rental, UpdateRentalDTO>().ReverseMap();
        }
    }
}
