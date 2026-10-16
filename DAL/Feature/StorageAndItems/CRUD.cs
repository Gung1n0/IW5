using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    private readonly AppDbContext _context;

    public PolozkaService(AppDbContext context)
    {
        _context = context;
    }

    //create Polozka
    public async Task<int> CreatePolozkaAsync(CreatePolozkaDTO Dto, int CurrentUserId)
    {
        var Entity = new PolozkaEntity
        {
            Name = Dto.Name,
            Url = Dto.Url,
            Amount = Dto.Amount,
            DateExpiration = Dto.DateExpiration,
            Category = Dto.Category,
            Text = Dto.Text,
            PlaceId = Dto.PlaceId,
            UserId = CurrentUserId
        };

        _context.Polozky.Add(Entity);

        await _context.SaveChangesAsync();

        return entity.Id;
    }

    //get all Polozkas
    public async Task<List<PolozkaDTO>> GetAllPolzka()
    {
        return await _context.Polozky.Select(Entity => new PolozkaDTO
                {
                    Id = Entity.Id,
                    Name = Entity.Name,
                    Url = Entity.Url,
                    Amount = Entity.Amount,
                    DateExpiration = Entity.DateExpiration,
                    Category = Entity.Category,
                    Text = Entity.Text,
                    UserId = Entity.UserId,
                    PlaceId = Entity.PlaceId
                }.ToListAsync();
    }

    //updates existing Polozka
    public async Task<PolozkaDTO> UpdatePolozka(int CurrentId, PolozkaEntity UpdatedPolozka)
    {
        PolozkaEntity Polozka = await _context.Polozky.FirstOrDefaultAsync(p => p.Id == CurrentId);
        
        if (Polozka == null)
            return null;

        Polozka.Name = UpdatedPolozka.Name;
        Polozka.Url = UpdatedPolozka.Url;
        Polozka.Amount = UpdatedPolozka.Amount;
        Polozka.DateExpiration = UpdatedPolozka.DateExpiration;
        Polozka.Category = UpdatedPolozka.Category;
        Polozka.Text = UpdatedPolozka.Text;
        Polozka.UserId = UpdatedPolozka.UserId;
        Polozka.PlaceId = UpdatedPolozka.PlaceId;
        //Polozka.user = UpdatedPolozka.user; 
        //Polozka.place = UpdatedPolozka.place;

        await _context.SaveChangesAsync();

        var ResultDto = new PolozkaDTO
        {
            Id = Polozka.Id,
            Name = Polozka.Name,
            Url = Polozka.Url,
            Amount = Polozka.Amount,
            DateExpiration = Polozka.DateExpiration,
            Category = Polozka.Category,
            Text = Polozka.Text,
            UserId = Polozka.UserId,
            PlaceId = Polozka.PlaceId
        };

        return ResultDto;
    }

    //deletes Polozka from db
    public async Task<void> RemovePolozka(int CurrentId)
    {
        PolozkaEntity Polozka = await _context.Polozky.FirstOrDefaultAsync(p => p.Id == CurrentId);
        
        if (Polozka == null)
            return;

        _context.Polozky.Remove(Polozka);
        await _context.SaveChangesAsync();
    }
}