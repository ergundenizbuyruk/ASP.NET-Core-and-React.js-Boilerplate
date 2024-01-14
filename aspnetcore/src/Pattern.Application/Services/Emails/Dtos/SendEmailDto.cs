namespace Pattern.Application.Services.Emails.Dtos
{
	public class SendEmailDto
	{
		public List<string> To { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public bool BodyIsHtml { get; set; }
		public List<AttachmentDto> Attachments { get; set; }
	}

	public class AttachmentDto
	{
		public string Name { get; set; }
		public string MediaType { get; set; }
		public string MediaSubtype { get; set; }
		public Byte[] Data { get; set; }
	}
}
