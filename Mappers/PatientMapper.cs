using CuraMundi.Application.BLL.Dto;
using CuraMundi.Domain.Entities;
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
                Nom=patientCreateDto.Nom,
                Prenom=patientCreateDto.Prenom,
                Email=patientCreateDto.Email,
                UserName=patientCreateDto.Email,
                PhoneNumber=patientCreateDto.Telephone,
              

            };
        }
        public static PatientCreateDto ToPatientDto(this Patient patient)
        {
            return new PatientCreateDto
            {
                Id=patient.Id,
                Adresse = patient.Adresse,
                DateNaiss = patient.DateNaiss,
                Nom=patient.Nom,
                Prenom=patient.Prenom,
                Email=patient.Email,
                Telephone=patient.PhoneNumber,
                Role="Patient"
                
            };
        }

    }
}
