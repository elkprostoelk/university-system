using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UniversitySystem.Data.Entities;
using UniversitySystem.Data.Interfaces;
using UniversitySystem.Services.Dtos;
using UniversitySystem.Services.Enums;
using UniversitySystem.Services.Interfaces;

namespace UniversitySystem.Services.ServiceImplementations
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public LessonService(
            ILessonRepository lessonRepository,
            IStudentRepository studentRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _lessonRepository = lessonRepository;
            _studentRepository = studentRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResult> CreateLesson(NewLessonDTO newLessonDto)
        {
            var result = new ServiceResult();
            var participants = await _studentRepository.GetStudents(newLessonDto.ParticipantIds);
            var lesson = new Lesson
            {
                Name = newLessonDto.Name,
                ScheduledOn = newLessonDto.ScheduledOn,
                LessonType = newLessonDto.LessonType,
                Participants = participants,
                TeacherId = newLessonDto.TeacherId
            };
            var containsTeacher = lesson.TeacherId > 0;
            var containsStudents = participants.Any();
            if (!containsTeacher)
            {
                result.IsSuccessful = false;
                result.Errors.Add("NoTeacher", "Lesson should contain a teacher!");
            }

            if (!containsStudents)
            {
                result.IsSuccessful = false;
                result.Errors.Add("NoStudents", "Lesson should contain at least one student!");
            }

            if (result.IsSuccessful)
            {
                await _lessonRepository.AddLessonAsync(lesson);
            }

            return result;
        }

        public async Task<ServiceResult> EditLesson(long lessonId, EditLessonDTO editLessonDto)
        {
            var result = new ServiceResult();
            var lesson = await _lessonRepository.GetLessonAsync(lessonId);
            if (lesson is null)
            {
                result.IsSuccessful = false;
                result.Errors.Add("LessonNotFound", "Lesson was not found!");
            }
            else
            {
                lesson.Name = editLessonDto.Name;
                lesson.ScheduledOn = editLessonDto.ScheduledOn;
                lesson.LessonType = editLessonDto.LessonType;
                lesson.Participants = await _studentRepository.GetStudents(editLessonDto.Participants);
                lesson.TeacherId = editLessonDto.TeacherId;
                await _lessonRepository.UpdateLessonAsync(lesson);
            }

            return result;
        }

        public async Task<ServiceResult> DeleteLesson(long lessonId)
        {
            var result = new ServiceResult();
            var lesson = await _lessonRepository.GetLessonAsync(lessonId);
            if (lesson is null)
            {
                result.IsSuccessful = false;
                result.Errors.Add("LessonNotFound", "Lesson was not found!");
            }
            else
            {
                await _lessonRepository.RemoveLessonAsync(lesson);
            }

            return result;
        }

        public async Task<ServiceResult> CountAbsences(AbsencePeriods duration, long? userId = null)
        {
            var result = new ServiceResult();
            var now = DateTime.Now;
            var limitDate = duration switch
            {
                AbsencePeriods.Daily => now.AddDays(-1),
                AbsencePeriods.Week => now.AddDays(-7),
                AbsencePeriods.Month => now.AddMonths(-1),
                AbsencePeriods.Year => now.AddYears(-1),
                _ => now
            };
            var filteredLessons = await _lessonRepository.GetLessonsAsync(limitDate);
            var absencesCount = filteredLessons.SelectMany(l => l.LessonParticipants
                        .Where(lp => !lp.Present),
                    (l, lp) => new LessonParticipantDTO {LessonId = l.Id, ParticipantId = lp.ParticipantId})
                .GroupBy(lp => lp.ParticipantId, (partId, abs) =>
                    new AbsencesCountDTO {ParticipantId = partId, AbsencesCount = abs.Count()});
            result.ResultObject = absencesCount;
            if (!userId.HasValue)
            {
                return result;
            }

            var user = await _userRepository.GetUser(userId.Value);
            if (user?.StudentId != null)
            {
                result.ResultObject = absencesCount.SingleOrDefault(ac => ac.ParticipantId == userId);
            }
            else
            {
                result.IsSuccessful = false;
                result.Errors.Add("UserNotFound", "User was not found!");
            }

            return result;
        }

        public async Task<ServiceResult> MarkPresence(long lessonId, long userId)
        {
            var result = new ServiceResult();
            var lesson = await _lessonRepository.GetLessonAsync(lessonId);
            if (lesson is null)
            {
                result.IsSuccessful = false;
                result.Errors.Add("LessonNotFound", "Lesson was not found!");
            }
            else
            {
                var user = await _userRepository.GetUser((int)userId);
                if (user is null)
                {
                    result.IsSuccessful = false;
                    result.Errors.Add("UserNotFound", "User was not found!");
                }
                else
                {
                    var present = await _lessonRepository.MarkPresenceAsync(lesson, user.StudentRole);
                    result.ResultObject = new { userId, present };
                }
            }

            return result;
        }

        public async Task<ServiceResult> GetSchedule(long? userId, DateTime startDate, DateTime endDate)
        {
            endDate = endDate.Add(TimeSpan.Parse("23:59:59.9999999"));
            var result = new ServiceResult();
            var weekLessons = await _lessonRepository.GetLessonsAsync(startDate, endDate, userId);
            result.ResultObject = weekLessons.GroupBy(wl => wl.ScheduledOn.TimeOfDay,
                (lessonTime, lessons) =>
                {
                    var enumerable = lessons.ToList();
                    return new WorkWeekDTO
                    {
                        LessonTime = new DateTime(lessonTime.Ticks),
                        MondayLesson =
                            _mapper.Map<CompactLessonDTO>(enumerable.SingleOrDefault(l =>
                                l.ScheduledOn.DayOfWeek == DayOfWeek.Monday)),
                        TuesdayLesson =
                            _mapper.Map<CompactLessonDTO>(enumerable.SingleOrDefault(l =>
                                l.ScheduledOn.DayOfWeek == DayOfWeek.Tuesday)),
                        WednesdayLesson =
                            _mapper.Map<CompactLessonDTO>(enumerable.SingleOrDefault(l =>
                                l.ScheduledOn.DayOfWeek == DayOfWeek.Wednesday)),
                        ThursdayLesson =
                            _mapper.Map<CompactLessonDTO>(enumerable.SingleOrDefault(l =>
                                l.ScheduledOn.DayOfWeek == DayOfWeek.Thursday)),
                        FridayLesson =
                            _mapper.Map<CompactLessonDTO>(enumerable.SingleOrDefault(l =>
                                l.ScheduledOn.DayOfWeek == DayOfWeek.Friday)),
                    };
                });
            return result;
        }

        public async Task<ServiceResult> GetLessonsForAdminPanel()
        {
            var result = new ServiceResult();
            var lessons = await _lessonRepository.GetLessonsAsync();
            result.ResultObject = lessons.Select(l => _mapper.Map<LessonDTO>(l));
            return result;
        }

        public async Task<ServiceResult> GetLesson(long lessonId)
        {
            var result = new ServiceResult();
            var lesson = await _lessonRepository.GetLessonAsync(lessonId);
            if (lesson is null)
            {
                result.IsSuccessful = false;
                result.Errors.Add("LessonNotFound", "Lesson was not found!");
            }
            else
            {
                result.ResultObject = new
                {
                    lesson.Name,
                    lesson.LessonType,
                    lesson.ScheduledOn,
                    teacher = lesson.Teacher.User.FullName,
                    participants = lesson.Participants
                        .Select(s => new {s.Id, name = $"{s.User.FirstName} {s.User.LastName}"})
                };
            }
            return result;
        }
    }
}