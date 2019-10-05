using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Dtos;
using CSharpFunctionalExtensions;
using Logic.Students;
using Logic.Utils;
using Microsoft.AspNetCore.Mvc;
using Logic.AppServices;

namespace Api.Controllers
{
    [Route("api/students")]
    public sealed class StudentController : BaseController
    {
        private readonly Messages _messages;

        public StudentController(Messages messages)
        {
            _messages = messages;
        }

        [HttpGet]
        public IActionResult GetList(string enrolled, int? number)
        {
            GetListQuery query = new GetListQuery(enrolled, number);
            List<StudentDto> dtos = _messages.Dispatch(query);
            return Ok(dtos);
        }
        
        [HttpPost]
        public IActionResult Register([FromBody] NewStudentDto dto)
        {
            RegisterCommand command = new RegisterCommand(dto.Name, dto.Email, dto.Course1, dto.Course1Grade, dto.Course2, dto.Course2Grade);
            Result result = _messages.Dispatch(command);
            return FromResult(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Unregister(long id)
        {
            UnregisterCommand command = new UnregisterCommand(id);
            Result result = _messages.Dispatch(command);
            return FromResult(result);
        }

        [HttpPost("{id}/enrollments")]
        public IActionResult Enroll(long id, [FromBody] StudentEnrollmentDto dto)
        {
            EnrollCommand command = new EnrollCommand(id, dto.Course, dto.Grade);
            Result result = _messages.Dispatch(command);
            return FromResult(result);
        }

        [HttpPut("{id}/enrollments/{enrollmentNumber}")]
        public IActionResult Transfer(long id, int enrollmentNumber, [FromBody] StudentTransferDto dto)
        {
            TransferCommand command = new TransferCommand(id, dto.Course, dto.Grade, enrollmentNumber - 1);
            Result result = _messages.Dispatch(command);
            return FromResult(result);
        }

        [HttpPost("{id}/enrollments/{enrollmentNumber}/deletion")]
        public IActionResult Disenroll(long id, int enrollmentNumber, [FromBody] StudentDisenrollmentDto dto)
        {
            DisenrollCommand command = new DisenrollCommand(id, enrollmentNumber, dto.Comment);
            Result result = _messages.Dispatch(command);
            return FromResult(result);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePersonalInfo(long id, [FromBody] StudentPersonalInfoDto dto)
        {
            EditPersonalInfoCommand command = new EditPersonalInfoCommand(id, dto.Name, dto.Email);
            Result result = _messages.Dispatch(command);
            return FromResult(result);
        }
    }
}
