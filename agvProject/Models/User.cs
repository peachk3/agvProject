using System;

namespace agvProject.Models
{
    public enum UserRole
    {
        User,
        Admin,
        Manager
    }

    public class User
    {
        public int Id { get; set; }
        public string UserId { get; set; } //로그인용 아이디 (관리자가 설정)
        public string UserName { get; set; } //사용자 이름
        public string HashedPassword { get; set; } // 해싱된 비밀번호
        public string? Department{ get; set; } //부서
        public string? Position { get; set; } //직급
        public UserRole Role { get; set; } //사용자 권한
        public bool IsActive { get; set; } //사용자 활성화 여부
        public DateTime CreatedAt { get; set; } //생성일
        public DateTime? LastLoginAt { get; set; } //마지막 로그인 시간
        //JWT 토큰 관련 속성
        public string RefreshToken { get; set; } //리프레시 토큰
        public DateTime? RefreshTokenExpiryTime { get; set; } //리프레시 토큰 만료시간
    }
}