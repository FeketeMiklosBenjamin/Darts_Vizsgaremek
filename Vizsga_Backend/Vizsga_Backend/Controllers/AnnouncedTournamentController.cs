using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vizsga_Backend.Models.MatchModels;
using Vizsga_Backend.Models.TournamentModels;
using Vizsga_Backend.Services;
using VizsgaBackend.Services;
using Vizsga_Backend.Models.MessageModels;

namespace Vizsga_Backend.Controllers
{
    [Route("api/announced_tournaments")]
    [ApiController]
    public class AnnouncedTournamentController : ControllerBase
    {
        private readonly AnnouncedTournamentService _service;
        private readonly MatchHeaderService _matchHeaderService;
        private readonly UsersTournamentStatService _usersTournamentStatService;
        private readonly MatchService _matchService;
        private readonly MessageService _messageService;
        private readonly Cloudinary _cloudinary;

        public AnnouncedTournamentController(AnnouncedTournamentService service, MatchHeaderService matchHeaderService, UsersTournamentStatService usersTournamentStatService, MatchService matchService, MessageService messageService, Cloudinary cloudinary)
        {
            _service = service;
            _matchHeaderService = matchHeaderService;
            _usersTournamentStatService = usersTournamentStatService;
            _matchService = matchService;
            _messageService = messageService;
            _cloudinary = cloudinary;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllAnnouncedTournament()
        {
            try
            {
                var userRole = User.FindFirstValue(ClaimTypes.Role);

                if (userRole == null)
                {
                    return Unauthorized(new {message = "Nincs bejelentkezve!"});
                }

                var tournaments = await _service.GetAnnouncedTournamentsWithPlayersAsync();

                if (userRole == "2")
                {
                    var result = tournaments.Select(item => new
                    {
                        item.Id,
                        item.HeaderId,
                        joinStartDate = TimeZoneInfo.ConvertTimeFromUtc(item.JoinStartDate, TimeZoneInfo.Local).ToString("yyyy.MM.dd. HH:mm"),
                        joinEndDate = TimeZoneInfo.ConvertTimeFromUtc(item.JoinEndDate, TimeZoneInfo.Local).ToString("yyyy.MM.dd. HH:mm"),
                        item.MaxPlayerJoin,
                        matchHeader = new
                        {
                            item.MatchHeader!.Name,
                            item.MatchHeader.Level,
                            item.MatchHeader.SetsCount,
                            item.MatchHeader.LegsCount,
                            item.MatchHeader.StartingPoint,
                            item.MatchHeader.BackroundImageUrl,
                            tournamentStartDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)item.MatchHeader.TournamentStartDate!, TimeZoneInfo.Local).ToString("yyyy.MM.dd."),
                            tournamentEndDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)item.MatchHeader.TournamentEndDate!, TimeZoneInfo.Local).ToString("yyyy.MM.dd."),
                            matches = item.MatchHeader.MatchDates.Select((date, index) => new KeyValuePair<string, string>($"match{index + 1}", TimeZoneInfo.ConvertTimeFromUtc((DateTime)date, TimeZoneInfo.Local).ToString("yyyy.MM.dd. HH:mm"))).ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                        },
                        registeredPlayers = item.RegisteredPlayers.Select(player => new
                        {
                            player.Id,
                            player.Username,
                            player.EmailAddress
                        })
                    }).ToList();
                    return Ok(result);
                }
                else
                {
                    var result = tournaments.Select(item => new
                    {
                        item.Id,
                        item.HeaderId,
                        joinStartDate = TimeZoneInfo.ConvertTimeFromUtc(item.JoinStartDate, TimeZoneInfo.Local).ToString("yyyy.MM.dd. HH:mm"),
                        joinEndDate = TimeZoneInfo.ConvertTimeFromUtc(item.JoinEndDate, TimeZoneInfo.Local).ToString("yyyy.MM.dd. HH:mm"),
                        item.MaxPlayerJoin,
                        matchHeader = new
                        {
                            item.MatchHeader!.Name,
                            item.MatchHeader.Level,
                            item.MatchHeader.SetsCount,
                            item.MatchHeader.LegsCount,
                            item.MatchHeader.StartingPoint,
                            item.MatchHeader.BackroundImageUrl,
                            tournamentStartDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)item.MatchHeader.TournamentStartDate!, TimeZoneInfo.Local).ToString("yyyy.MM.dd."),
                            tournamentEndDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)item.MatchHeader.TournamentEndDate!, TimeZoneInfo.Local).ToString("yyyy.MM.dd."),
                            matches = item.MatchHeader.MatchDates.Select((date, index) => new KeyValuePair<string, string>($"match{index + 1}", TimeZoneInfo.ConvertTimeFromUtc((DateTime)date, TimeZoneInfo.Local).ToString("yyyy.MM.dd. HH:mm"))).ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                        },
                        registeredPlayers = item.RegisteredPlayers.Count()
                    }).ToList();
                    return Ok(result);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A lekérés során hiba történt." });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAnnouncedTournament([FromBody] TournamentCreate datas)
        {
            try
            {
                var userRole = User.FindFirstValue(ClaimTypes.Role);

                if (userRole != "2")
                {
                    return Unauthorized(new { message = "Csak admin tud meghírdetni új versenyeket!" });
                }

                if (datas == null)
                {
                    return BadRequest(new { message = "Nem adott meg adatokat!" });
                }

                if (String.IsNullOrWhiteSpace(datas.Name))
                {
                    return BadRequest(new { message = "Nem lehet üres a cím!" });
                }

                datas.ConvertToUtc();

                string badRequestMessage = _service.ValidateCreateDatas(datas);

                if (badRequestMessage != "")
                {
                    return BadRequest(new { message = badRequestMessage });
                }

                datas.MatchDates.Sort();

                datas.Password = BCrypt.Net.BCrypt.HashPassword(datas.Password);

                var matchHeader = new MatchHeader()
                {
                    Name = datas.Name,
                    DeleteDate = datas.MatchDates[datas.MatchDates.Count() - 1].AddDays(30),
                    SetsCount = (int)datas.SetsCount!,
                    LegsCount = (int)datas.LegsCount!,
                    StartingPoint = (int)datas.StartingPoint!,
                    JoinPassword = datas.Password!,
                    BackroundImageUrl = "",
                    TournamentStartDate = datas.MatchDates[0],
                    TournamentEndDate = datas.MatchDates[datas.MatchDates.Count() - 1],
                    MatchDates = datas.MatchDates,
                    IsDrawed = false
                };

                switch (datas.Level)
                {
                    case "Haladó":
                        matchHeader.Level = "Advanced";
                        break;
                    case "Profi":
                        matchHeader.Level = "Professional";
                        break;
                    case "Bajnok":
                        matchHeader.Level = "Champion";
                        break;
                    default:
                        matchHeader.Level = "Amateur";
                        break;
                }

                await _matchHeaderService.CreateAsync(matchHeader);


                var announcedTournament = new AnnouncedTournament() 
                {
                    HeaderId = matchHeader.Id,
                    JoinStartDate = (DateTime)datas.JoinStartDate!,
                    JoinEndDate = (DateTime)datas.JoinEndDate!,
                    MaxPlayerJoin = (int)datas.MaxPlayerJoin!
                };


                await _service.CreateTournamentAsync(announcedTournament);

                return Ok(new {
                    message = "Sikeres létrehozás!",
                    headerId = matchHeader.Id
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Az elküldés során hiba történt." });
                throw;
            }
        }


        [HttpPut("background/upload/{matchHeaderId}")]
        [Authorize]
        public async Task<IActionResult> UploadBackgroundImage(string matchHeaderId, IFormFile file)
        {
            try
            {
                var userRole = User.FindFirstValue(ClaimTypes.Role);

                if (userRole != "2")
                {
                    return Unauthorized(new { message = "Csak admin versenyeknek hátteret állítani!" });
                }

                if (file == null || file.Length == 0)
                    return BadRequest(new { message = "Nem adott meg fájlt!" });

                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "darts_background_pictures"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                
                var header = await _matchHeaderService.GetByIdAsync(matchHeaderId);
                if (header == null)
                {
                    return NotFound(new { message = $"A mérkőzés fejléc az ID-vel ({matchHeaderId}) nem található." });
                }

                await _matchHeaderService.DeleteBackgroundImageAsync(header);
                await _matchHeaderService.SaveBackgroundImageAsync(matchHeaderId, uploadResult.SecureUrl.ToString());

                return Ok(new { backgroundImageUrl = uploadResult.SecureUrl.ToString() });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A feltöltés során hiba történt." });
            }
        }

        [HttpPost("register/{tournamentId}")]
        [Authorize]
        public async Task<IActionResult> RegisterForAnnouncedTournament(string tournamentId)
        {
            try
            {
                var userRole = User.FindFirstValue(ClaimTypes.Role);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    return Unauthorized(new { message = "Nincs bejelentkezve!" });
                }

                if (userRole != "1")
                {
                    return Unauthorized(new { message = "Csak felhasználó tud regisztrálni versenyekre!" });
                }

                var tournament = await _service.GetAnnouncedTournamentByIdAsync(tournamentId);
                var userStat = await _usersTournamentStatService.GetTournamentByUserIdAsync(userId);

                if (tournament == null)
                {
                    return NotFound(new { message = $"A verseny az ID-vel ({tournamentId}) nem található." });
                }

                if (tournament.JoinStartDate > DateTime.UtcNow)
                {
                    return BadRequest(new { message = "A versenyre a jelentkezés még nem kezdődött el!" });
                }

                if (tournament.JoinEndDate < DateTime.UtcNow)
                {
                    return BadRequest(new { message = "A versenyre a jelentkezés már befejeződött!" });
                }

                int JoinedPlayers = await _service.JoinedPlayerToTournamentCountAsync(tournamentId);

                if (tournament.MaxPlayerJoin <= JoinedPlayers)
                {
                    return Conflict(new { message = "A verseny férőhelye betelt!" });
                }

                var matchHeader = await _matchHeaderService.GetByIdAsync(tournament.HeaderId);

                if (matchHeader == null)
                {
                    return NotFound(new { message = $"A mérkőzés fejléc az ID-vel ({tournamentId}) nem található." });
                }

                if (matchHeader.Level != userStat.Level)
                {
                    return BadRequest(new { message = $"A szinted ({userStat.Level}) nem megfelelő a verseny jelentkezési szintjének ({matchHeader.Level})" });
                }

                if (await _service.DoesPlayerJoinedThisTournamentAsync(tournamentId, userId))
                {
                    return Conflict(new { message = "A felhasználó már jelentkezett a versenyre!" });
                }

                var newConnection = new PlayerTournament
                {
                    AnnoucedTournamentId = tournamentId,
                    UserId = userId,
                    JoinedNumber = JoinedPlayers + 1
                };

                await _service.CreateRegisterAsync(newConnection);

                return Created();

            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A jelentkezés során hiba történt." });
                throw;
            }
        }

        [HttpDelete("draw/{tournamentId}")]
        [Authorize]
        public async Task<IActionResult> TournamentDraw(string tournamentId)
        {
            try
            {
                var userRole = User.FindFirstValue(ClaimTypes.Role);

                if (userRole != "2")
                {
                    return Unauthorized(new {message = "Csak admin tud verseny mérkőzéseket sorsolni!"});
                }

                var tournament = await _service.GetAnnouncedTournamentByIdAsync(tournamentId);

                if (tournament == null)
                {
                    return NotFound(new { message = $"A verseny az ID-vel ({tournamentId}) nem található." });
                }

                var matchHeader = await _matchHeaderService.GetByIdAsync(tournament.HeaderId);

                if (matchHeader == null)
                {
                    return NotFound(new { message = $"A mérkőzés fejléc az ID-vel ({tournamentId}) nem található." });
                }

                Random random = new Random();

                var joinedPlayers = await _service.GetJoinedPlayersAsync(tournamentId);

                var newPlayersCount = tournament.MaxPlayerJoin;

                while (joinedPlayers.Count() < newPlayersCount && newPlayersCount >= 4)
                {
                    newPlayersCount = newPlayersCount / 2;
                }

                if (newPlayersCount < 4)
                {
                    newPlayersCount = 0;
                }

                if (newPlayersCount != tournament.MaxPlayerJoin)
                {
                    Message rejectMessage = new Message
                    {
                        FromId = null,
                        Title = $"Versenyre való jelentkezés törlése - [{matchHeader.Name}]",
                        Text = $"Kedves Felhasználó!\r\n\r\nSajnálattal értesítünk, hogy a {matchHeader.Name} versenyre történő jelentkezésedet töröltük, mivel a versenyhez nem érkezett elegendő jelentkező.\r\n\r\nEz nem a Te hibád, és bízunk benne, hogy a következő verseny jelentkezésednél már elegendő jelentkező lesz a lebonyolításhoz!” Ha bármilyen kérdésed van, keresd bátran ügyfélszolgálatunkat.\r\n\r\nÜdvözlettel,\r\nAdmin",
                        SendDate = DateTime.UtcNow
                    };

                    if (newPlayersCount == 0)
                    {
                        foreach (var player in joinedPlayers)
                        {
                            rejectMessage.ToId = player.UserId;
                            await _messageService.CreateAsync(rejectMessage);
                        }

                        await _matchHeaderService.DeleteMatchHeaderAsync(tournament.HeaderId);

                        await _service.DeleteAllRegisterAndTournamentAsync(tournamentId);

                        return NoContent();
                    }
                    else
                    {
                        foreach (var player in joinedPlayers)
                        {
                            if (player.JoinedNumber > newPlayersCount)
                            {
                                rejectMessage.ToId = player.UserId;
                                await _messageService.CreateAsync(rejectMessage);
                                joinedPlayers.Remove(player);
                            }
                        }
                    }
                }

                int playerOneIndex;
                int playerTwoIndex;
                int rowNumber = 1;

                Match newMatch;

                Message successMessage;

                do
                {
                    newMatch = new Match
                    {
                        HeaderId = tournament.HeaderId,
                        Status = "Pedding",
                        StartDate = matchHeader.MatchDates[0],
                        RemainingPlayer = tournament.MaxPlayerJoin,
                        PlayerOneStatId = null,
                        PlayerTwoStatId = null
                    };
                    playerOneIndex = random.Next(0, joinedPlayers.Count());
                    newMatch.PlayerOneId = joinedPlayers[playerOneIndex].UserId;
                    joinedPlayers.RemoveAt(playerOneIndex);

                    successMessage = new Message
                    {
                        FromId = null,
                        Title = $"Verseny sorsolása megtörtént - [{matchHeader.Name}]",
                        Text = $"Kedves Felhasználó!\r\n\r\nA(z) {matchHeader.Name} verseny sorsolása megtörtént\r\n\r\nA sorsolás végeredményét, a mérkőzés időpontját és ellenfelét is megtudja nézni a ... oldalunkon.\r\n\r\nÜdvözlettel,\r\nAdmin",
                        SendDate = DateTime.UtcNow
                    };

                    successMessage.ToId = newMatch.PlayerOneId;
                    await _messageService.CreateAsync(successMessage);

                    playerTwoIndex = random.Next(0, joinedPlayers.Count());
                    newMatch.PlayerTwoId = joinedPlayers[playerTwoIndex].UserId;
                    joinedPlayers.RemoveAt(playerTwoIndex);

                    successMessage = new Message
                    {
                        FromId = null,
                        Title = $"Verseny sorsolása megtörtént - [{matchHeader.Name}]",
                        Text = $"Kedves Felhasználó!\r\n\r\nA(z) {matchHeader.Name} verseny sorsolása megtörtént\r\n\r\nA sorsolás végeredményét, a mérkőzés időpontját és ellenfelét is megtudja nézni a ... oldalunkon.\r\n\r\nÜdvözlettel,\r\nAdmin",
                        SendDate = DateTime.UtcNow
                    };

                    successMessage.ToId = newMatch.PlayerTwoId;
                    await _messageService.CreateAsync(successMessage);

                    newMatch.RowNumber = rowNumber;

                    await _matchService.CreateMatchAsync(newMatch);

                    rowNumber++;
                }
                while (joinedPlayers.Count() != 0);

                await _service.DeleteAllRegisterAndTournamentAsync(tournamentId);

                await _matchHeaderService.SetDrawedAsync(matchHeader.Id);

                return NoContent();

            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "A sorsolás során hiba történt." });
                throw;
            }
        }

    }
}
