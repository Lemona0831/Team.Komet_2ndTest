# Buggy User Management API

이 프로젝트는 개발팀 2차 실기 2번 과제인 **Buggy User Management API 수정 과제**입니다.

제공된 ASP.NET Core 스타터 프로젝트를 기반으로, 사용자 생성·조회·권한 변경·삭제 API에 포함되어 있던 버그와 설계 문제를 수정했습니다.

주요 수정 내용은 다음과 같습니다.

* 상황에 맞는 HTTP 상태 코드 반환
* 존재하지 않는 사용자 요청 처리
* 삭제된 사용자 조회 제외
* 사용자 생성 입력값 검증
* role 값 검증
* 이메일 중복 검사
* 비밀번호 응답 노출 방지
* UserResponse 매핑 오류 수정

## 1. 실행 방법

이 프로젝트는 .NET 10 SDK 기준으로 실행됩니다.

프로젝트 폴더로 이동한 뒤 아래 명령어를 실행합니다.

```bash
dotnet restore
dotnet run
```

실행 후 콘솔에 출력되는 주소를 기준으로 API를 호출합니다.

예시:

```text
http://localhost:60273
https://localhost:60272
```

포트 번호는 실행 환경에 따라 달라질 수 있습니다.

Postman 또는 curl을 사용하여 API를 테스트할 수 있습니다.

## 2. 사용 기술

* C#
* ASP.NET Core
* .NET 10 SDK
* Minimal API
* In-Memory List 저장 방식

## 3. 수정한 API 목록

| Method | URL                | 수정 내용                                                                               |
| ------ | ------------------ | ----------------------------------------------------------------------------------- |
| POST   | `/users`           | 사용자 생성 시 입력값 검증, 이메일 중복 검사, id 자동 증가, `201 Created` 반환, password 응답 제외              |
| GET    | `/users`           | 삭제되지 않은 사용자만 조회하도록 수정, password 응답 제외, name 매핑 오류 수정                                |
| GET    | `/users/{id}`      | 존재하지 않거나 삭제된 사용자 조회 시 `404 Not Found` 반환, password 응답 제외                            |
| PATCH  | `/users/{id}/role` | role 값 검증 추가, 존재하지 않거나 삭제된 사용자 처리, 잘못된 role 요청 시 `400 Bad Request` 반환               |
| DELETE | `/users/{id}`      | 사용자 논리 삭제 처리, 삭제 성공 시 `204 No Content`, 존재하지 않거나 이미 삭제된 사용자 요청 시 `404 Not Found` 반환 |

## 4. 예시 요청/응답

### 4-1. 사용자 생성

```http
POST /users
```

요청 예시:

```json
{
  "email": "lemona@example.com",
  "password": "1234",
  "name": "lemona",
  "role": "USER"
}
```

성공 응답:

```http
201 Created
```

```json
{
  "id": 1,
  "email": "lemona@example.com",
  "name": "lemona",
  "role": "USER"
}
```

비밀번호는 응답에 포함되지 않습니다.

입력값이 비어 있거나 role 값이 잘못된 경우:

```http
400 Bad Request
```

이미 존재하는 이메일을 사용하는 경우:

```http
409 Conflict
```

이메일 중복 검사는 대소문자를 구분하지 않습니다.

예를 들어 아래 두 이메일은 같은 이메일로 처리됩니다.

```text
Test@example.com
test@example.com
```

---

### 4-2. 사용자 목록 조회

```http
GET /users
```

성공 응답:

```http
200 OK
```

```json
[
  {
    "id": 1,
    "email": "lemona@example.com",
    "name": "lemona",
    "role": "USER"
  }
]
```

삭제된 사용자는 목록에 포함되지 않습니다.

응답에 비밀번호는 포함되지 않습니다.

---

### 4-3. 사용자 단건 조회

```http
GET /users/1
```

성공 응답:

```http
200 OK
```

```json
{
  "id": 1,
  "email": "lemona@example.com",
  "name": "lemona",
  "role": "USER"
}
```

존재하지 않는 사용자 또는 삭제된 사용자를 조회하는 경우:

```http
404 Not Found
```

---

### 4-4. 사용자 권한 변경

```http
PATCH /users/1/role
```

요청 예시:

```json
{
  "role": "ADMIN"
}
```

성공 응답:

```http
200 OK
```

```json
{
  "id": 1,
  "email": "lemona@example.com",
  "name": "lemona",
  "role": "ADMIN"
}
```

role 값은 `USER`, `ADMIN`만 허용합니다.

잘못된 role 값이 들어온 경우:

```http
400 Bad Request
```

존재하지 않거나 삭제된 사용자의 권한을 변경하려는 경우:

```http
404 Not Found
```

---

### 4-5. 사용자 삭제

```http
DELETE /users/1
```

성공 응답:

```http
204 No Content
```

존재하지 않는 사용자 또는 이미 삭제된 사용자를 삭제하려는 경우:

```http
404 Not Found
```

삭제된 사용자는 이후 목록 조회와 단건 조회에서 보이지 않습니다.

## 5. 미구현 또는 아쉬운 부분

현재 과제 요구사항에 포함된 주요 버그 수정과 API 동작은 구현했습니다.

다만 현재 프로젝트는 메모리 `List`를 사용하므로 서버를 재시작하면 데이터가 초기화됩니다.
과제 조건상 메모리 저장 방식이 허용되어 있기 때문에 별도의 데이터베이스는 사용하지 않았습니다.

추후 개선한다면 다음 기능을 추가할 수 있습니다.

* SQLite 또는 다른 데이터베이스를 사용한 데이터 영속성 처리
* role 값을 문자열이 아닌 enum으로 관리
* 공통 에러 응답 DTO 추가
* 단위 테스트 또는 통합 테스트 추가
* Swagger/OpenAPI 문서화 추가
