﻿using CuraMundi.Application.BLL.Dto;
using CuraMundi.Domain.Entities;
using CuraMundi.Dto;
using CuraMundi.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuraMundi.Application.BLL.Mappers
{
    public static class PatientMapper
    {
        public static Patient ToPatient(this PatientCreateDto patientCreateDto)
        {
            return new Patient
            {
                
                Adresse = patientCreateDto.Adresse,
                DateNaiss = patientCreateDto.DateNaiss,
                Nom=patientCreateDto.Nom.ToUpperCase(),
                Prenom=patientCreateDto.Prenom.Capitalize(),
                Email=patientCreateDto.Email,
                UserName=patientCreateDto.Email,
                PhoneNumber=patientCreateDto.Telephone,
              

            };
        }
        //public static Patient ToPatientForUpdate(this PatientUpdateDto patientUpdateDto)
        //{
        //    return new Patient
        //    {
                
        //        Adresse = patientUpdateDto.Adresse,
        //        DateNaiss = patientUpdateDto.DateNaiss,
        //        Nom= patientUpdateDto.Nom,
        //        Prenom= patientUpdateDto.Prenom,
        //        //Email= patientUpdateDto.Email,
        //        //UserName= patientUpdateDto.Email,
        //        PhoneNumber= patientUpdateDto.Telephone,
              

        //    };
        //}
        public static PatientDetailDto ToPatientDto(this Patient patient)
        {
            return new PatientDetailDto
            {
                Id=patient.Id,
                Adresse = patient.Adresse,
                DateNaiss = patient.DateNaiss,
                Nom=patient.Nom.ToUpperCase(),
                Prenom=patient.Prenom.Capitalize(),
                Email=patient.Email,
                Telephone=patient.PhoneNumber,
                Role="Patient",
                FullName = patient.Nom.ToUpperCase() + " " + patient.Prenom.Capitalize()

            };
        }

    }
}
