using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vizsga_Backend.Models.MatchModels;
using Vizsga_Backend.Models.TournamentModels;
using Vizsga_Backend.Services;
using VizsgaBackend.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Vizsga_Backend.Controllers
{
    [Route("api/announced_tournaments")]
    [ApiController]
    public class AnnouncedTournamentController : ControllerBase
    {
        private readonly AnnouncedTournamentService _service;
        private readonly MatchHeaderService _matchHeaderService;
        private readonly UsersTournamentStatService _usersTournamentStatService;
        private readonly Cloudinary _cloudinary;

        public AnnouncedTournamentController(AnnouncedTournamentService service, MatchHeaderService matchHeaderService, UsersTournamentStatService usersTournamentStatService ,Cloudinary cloudinary)
        {
            _service = service;
            _matchHeaderService = matchHeaderService;
            _usersTournamentStatService = usersTournamentStatService;
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

                var tournaments = await _service.GetAnnouncedTournamentsWithPlayers();

                if (userRole == "2")
                {
                    var result = tournaments.Select(item => new
                    {
                        item.Id,
                        item.HeaderId,
                        item.RequiredLevel,
                        item.MaxLevel,
                        joinStartDate = TimeZoneInfo.ConvertTimeFromUtc(item.JoinStartDate, TimeZoneInfo.Local).ToString("yyyy.MM.dd. HH:mm"),
                        joinEndDate = TimeZoneInfo.ConvertTimeFromUtc(item.JoinEndDate, TimeZoneInfo.Local).ToString("yyyy.MM.dd. HH:mm"),
                        item.MaxPlayerJoin,
                        matchHeader = new
                        {
                            item.MatchHeader!.Name,
                            item.MatchHeader.SetsCount,
                            item.MatchHeader.LegsCount,
                            item.MatchHeader.StartingPoint,
                            item.MatchHeader.BackroundImageUrl,
                            tournamentStartDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)item.MatchHeader.TournamentStartDate!, TimeZoneInfo.Local).ToString("yyyy.MM.dd."),
                            tournamentEndDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)item.MatchHeader.TournamentEndDate!, TimeZoneInfo.Local).ToString("yyyy.MM.dd."),
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
                        item.RequiredLevel,
                        item.MaxLevel,
                        joinStartDate = TimeZoneInfo.ConvertTimeFromUtc(item.JoinStartDate, TimeZoneInfo.Local).ToString("yyyy.MM.dd. HH:mm"),
                        joinEndDate = TimeZoneInfo.ConvertTimeFromUtc(item.JoinEndDate, TimeZoneInfo.Local).ToString("yyyy.MM.dd. HH:mm"),
                        item.MaxPlayerJoin,
                        matchHeader = new
                        {
                            item.MatchHeader!.Name,
                            item.MatchHeader.SetsCount,
                            item.MatchHeader.LegsCount,
                            item.MatchHeader.StartingPoint,
                            item.MatchHeader.BackroundImageUrl,
                            tournamentStartDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)item.MatchHeader.TournamentStartDate!, TimeZoneInfo.Local).ToString("yyyy.MM.dd."),
                            tournamentEndDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)item.MatchHeader.TournamentEndDate!, TimeZoneInfo.Local).ToString("yyyy.MM.dd."),
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

                datas.Password = BCrypt.Net.BCrypt.HashPassword(datas.Password);

                var matchHeader = new MatchHeader()
                {
                    Name = datas.Name,
                    SetsCount = (int)datas.SetsCount!,
                    LegsCount = (int)datas.LegsCount!,
                    StartingPoint = (int)datas.StartingPoint!,
                    JoinPassword = datas.Password!,
                    BackroundImageUrl = "",
                    TournamentStartDate = datas.TournamentStartDate,
                    TournamentEndDate = datas.TournamentEndDate
                };

                await _matchHeaderService.CreateAsync(matchHeader);


                var announcedTournament = new AnnouncedTournament() 
                {
                    HeaderId = matchHeader.Id,
                    RequiredLevel = 0,
                    MaxLevel = 1499,
                    JoinStartDate = (DateTime)datas.JoinStartDate!,
                    JoinEndDate = (DateTime)datas.JoinEndDate!,
                    MaxPlayerJoin = (int)datas.MaxPlayerJoin!
                };

                switch (datas.Level)
                {
                    case "Haladó":
                        announcedTournament.RequiredLevel = 1500;
                        announcedTournament.MaxLevel = 4499;
                        break;
                    case "Profi":
                        announcedTournament.RequiredLevel = 4500;
                        announcedTournament.MaxLevel = 8999;
                        break;
                    case "Bajnok":
                        announcedTournament.RequiredLevel = 9000;
                        announcedTournament.MaxLevel = null;
                        break;
                    default:
                        announcedTournament.RequiredLevel = 0;
                        announcedTournament.MaxLevel = 1499;
                        break;
                }

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


        [HttpPut("background/upload{matchHeaderId}")]
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
                    return NotFound(new { message = $"A meccs fejléc az ID-vel ({matchHeaderId}) nem található." });
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

                var tournament = await _service.GetAnnouncedTournamentById(tournamentId);
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

                if (tournament.MaxPlayerJoin <= await _service.JoinedPlayerToTournamentCount(tournamentId))
                {
                    return Conflict(new { message = "A verseny férőhelye betelt!" });
                }

                if (tournament.RequiredLevel > userStat.DartsPoints || tournament.MaxLevel < userStat.DartsPoints)
                {
                    return BadRequest(new { message = $"A pontod ({userStat.DartsPoints}) {(tournament.RequiredLevel > userStat.DartsPoints ? "kisebb" : "nagyobb" )} a verseny jelentkezési szintnél ({tournament.RequiredLevel}-{tournament.MaxLevel})" });
                }

                if (await _service.DoesPlayerJoinedThisTournament(tournamentId, userId))
                {
                    return Conflict(new { message = "A felhasználó már jelentkezett a versenyre!" });
                }

                var newConnection = new PlayerTournament
                {
                    AnnoucedTournamentId = tournamentId,
                    UserId = userId
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


    }
}
