using BAL.Interfaces;
using DAL.IRepo;
using SharedModels.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class PeopleService : IPeopleService
    {
        private readonly IPeopleRepo _peopleRepo;
        
        public clsGlobal.enSaveMode SaveMode { get; set; }
        public virtual clsPerson People { get; set; }

        public PeopleService(IPeopleRepo peopleRepo)
        {
            _peopleRepo = peopleRepo;
            SaveMode = clsGlobal.enSaveMode.Add;
        }

        public async Task<bool> AddAsync(clsPerson person)
        {
            return await _peopleRepo.AddAsync(person);
        }

        public async Task<bool> UpdateAsync(clsPerson person)
        {
            return await _peopleRepo.UpdateAsync(person);
        }

        public async Task<bool> DeleteAsync(int personID)
        {
            return await _peopleRepo.DeleteAsync(personID);
        }

        public async Task<clsPerson> GetByIdAsync(int personID)
        {
            return await _peopleRepo.GetByIdAsync(personID);
        }

        public async Task<List<clsPerson>> GetAllAsync()
        {
            return await _peopleRepo.GetAllAsync();
        }

        public async Task<clsPerson> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _peopleRepo.GetByPhoneNumberAsync(phoneNumber);
        }

        public async Task<bool> Save()
        {
            if (SaveMode == clsGlobal.enSaveMode.Add)
            {
                var result = await AddAsync(People);
                if (result)
                    SaveMode = clsGlobal.enSaveMode.Update;
                return result;
            }
            else
            {
                return await UpdateAsync(People);
            }
        }
    }
}
