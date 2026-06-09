# Issue Tracker API

이 프로젝트는 개발팀 2차 실기 1번 과제인 **Issue Tracker REST API 구현 과제**입니다.

C#과 ASP.NET Core를 사용하여 간단한 이슈 관리 REST API를 구현했습니다.

이슈 생성, 목록 조회, 단건 조회, 상태 변경, 담당자 변경, 삭제, 요약 조회 기능을 제공합니다.

데이터는 SQLite에 저장되며, 서버를 재시작해도 기존 데이터가 유지됩니다.

## 1. 실행 방법

이 프로젝트는 .NET 10 SDK 기준으로 실행됩니다.

저장소를 클론한 뒤, 1번 과제 폴더로 이동합니다.

```bash
git clone <repository-url>
cd Team.Komet_2ndTest/task1-issue-tracker-api
```

Visual Studio를 사용하는 경우 `Issue Tracker.sln` 파일을 열어 실행할 수 있습니다.

CLI로 실행하는 경우, 솔루션 파일이 있는 위치에서 아래 명령어를 실행합니다.

```bash
dotnet restore
dotnet run --project "Issue Tracker/Issue Tracker.csproj"
```

또는 프로젝트 폴더로 직접 이동한 뒤 실행할 수도 있습니다.

```bash
cd "Issue Tracker"
dotnet restore
dotnet run
```

실행에 성공하면 콘솔에 아래와 비슷한 주소가 출력됩니다.

```text
Now listening on: https://localhost:7056
Now listening on: http://localhost:5085
```

Swagger가 활성화되어 있으므로, 브라우저에서 아래 주소로 접속하여 API를 테스트할 수 있습니다.

```text
https://localhost:7056/swagger
```

포트 번호는 실행 환경에 따라 달라질 수 있습니다.

따라서 실제 테스트 시에는 콘솔에 출력된 주소 뒤에 `/swagger`를 붙여 접속하면 됩니다.

## 2. 사용 기술

* C#
* ASP.NET Core
* .NET 10 SDK
* Entity Framework Core
* SQLite
* Swagger / OpenAPI
* JSON 요청/응답

## 3. 데이터베이스 설정 방법

본 프로젝트는 SQLite를 사용합니다.

애플리케이션 최초 실행 시 `issues.db` 파일이 자동으로 생성됩니다.

`Program.cs`에서 `Database.EnsureCreated()`를 사용하여 데이터베이스와 테이블을 생성합니다.

DB 파일은 프로젝트 실행 경로 기준으로 생성됩니다.

```text
issues.db
```

별도의 수동 DB 생성 작업은 필요하지 않습니다.

서버를 종료했다가 다시 실행해도 `issues.db` 파일이 유지되는 한 기존 이슈 데이터는 삭제되지 않습니다.

## 4. 구현한 API 목록

| Method | URL                     | 설명        |
| ------ | ----------------------- | --------- |
| POST   | `/issues`               | 이슈 생성     |
| GET    | `/issues`               | 이슈 목록 조회  |
| GET    | `/issues/{id}`          | 이슈 단건 조회  |
| PATCH  | `/issues/{id}/status`   | 이슈 상태 변경  |
| PATCH  | `/issues/{id}/assignee` | 이슈 담당자 변경 |
| DELETE | `/issues/{id}`          | 이슈 삭제     |
| GET    | `/issues/summary`       | 이슈 요약 조회  |

## 5. 예시 요청/응답

### 5-1. 이슈 생성

```http
POST /issues
```

요청 예시:

```json
{
  "title": "로그인 버그 수정",
  "description": "로그인 시 토큰이 만료되는 문제",
  "priority": "HIGH",
  "assignee": "lemona"
}
```

성공 응답:

```http
201 Created
```

```json
{
  "id": 1,
  "title": "로그인 버그 수정",
  "description": "로그인 시 토큰이 만료되는 문제",
  "priority": "HIGH",
  "status": "TODO",
  "assignee": "lemona",
  "createdAt": "2026-06-09T12:00:00Z",
  "updatedAt": "2026-06-09T12:00:00Z"
}
```

생성 시 `status`는 자동으로 `TODO`로 설정됩니다.

`createdAt`, `updatedAt`도 생성 시 자동으로 설정됩니다.

다음과 같은 경우에는 `400 Bad Request`를 반환합니다.

* title이 비어 있는 경우
* assignee가 비어 있는 경우
* priority가 `LOW`, `MEDIUM`, `HIGH` 중 하나가 아닌 경우

---

### 5-2. 이슈 목록 조회

```http
GET /issues
```

성공 응답:

```http
200 OK
```

```json
[
  {
    "id": 1,
    "title": "로그인 버그 수정",
    "description": "로그인 시 토큰이 만료되는 문제",
    "priority": "HIGH",
    "status": "TODO",
    "assignee": "lemona",
    "createdAt": "2026-06-09T12:00:00Z",
    "updatedAt": "2026-06-09T12:00:00Z"
  }
]
```

지원하는 Query String은 다음과 같습니다.

```http
GET /issues?status=TODO
GET /issues?priority=HIGH
GET /issues?assignee=lemona
GET /issues?sort=priority
GET /issues?status=TODO&priority=HIGH
GET /issues?status=TODO&sort=priority
```

`sort=priority`를 사용할 경우 우선순위는 아래 순서로 정렬됩니다.

```text
HIGH -> MEDIUM -> LOW
```

우선순위가 같으면 `id` 오름차순으로 정렬됩니다.

잘못된 `status`, `priority`, `sort` 값이 들어오면 `400 Bad Request`를 반환합니다.

---

### 5-3. 이슈 단건 조회

```http
GET /issues/1
```

성공 응답:

```http
200 OK
```

```json
{
  "id": 1,
  "title": "로그인 버그 수정",
  "description": "로그인 시 토큰이 만료되는 문제",
  "priority": "HIGH",
  "status": "TODO",
  "assignee": "lemona",
  "createdAt": "2026-06-09T12:00:00Z",
  "updatedAt": "2026-06-09T12:00:00Z"
}
```

해당 id의 이슈가 존재하지 않는 경우:

```http
404 Not Found
```

---

### 5-4. 이슈 상태 변경

```http
PATCH /issues/1/status
```

요청 예시:

```json
{
  "status": "DOING"
}
```

성공 응답:

```http
200 OK
```

```json
{
  "id": 1,
  "title": "로그인 버그 수정",
  "description": "로그인 시 토큰이 만료되는 문제",
  "priority": "HIGH",
  "status": "DOING",
  "assignee": "lemona",
  "createdAt": "2026-06-09T12:00:00Z",
  "updatedAt": "2026-06-09T12:10:00Z"
}
```

상태 변경 시 `updatedAt`이 갱신됩니다.

다음과 같은 경우에는 실패합니다.

| 상황                                             | 응답                |
| ---------------------------------------------- | ----------------- |
| 존재하지 않는 id                                     | `404 Not Found`   |
| status 값이 `TODO`, `DOING`, `DONE` 중 하나가 아님     | `400 Bad Request` |
| 이미 `DONE` 상태인 이슈를 `TODO` 또는 `DOING`으로 되돌리려는 경우 | `400 Bad Request` |

---

### 5-5. 이슈 담당자 변경

```http
PATCH /issues/1/assignee
```

요청 예시:

```json
{
  "assignee": "minsu"
}
```

성공 응답:

```http
200 OK
```

```json
{
  "id": 1,
  "title": "로그인 버그 수정",
  "description": "로그인 시 토큰이 만료되는 문제",
  "priority": "HIGH",
  "status": "DOING",
  "assignee": "minsu",
  "createdAt": "2026-06-09T12:00:00Z",
  "updatedAt": "2026-06-09T12:15:00Z"
}
```

담당자 변경 시 `updatedAt`이 갱신됩니다.

다음과 같은 경우에는 실패합니다.

| 상황                              | 응답                |
| ------------------------------- | ----------------- |
| 존재하지 않는 id                      | `404 Not Found`   |
| assignee가 비어 있음                 | `400 Bad Request` |
| 이미 `DONE` 상태인 이슈의 담당자를 변경하려는 경우 | `400 Bad Request` |

---

### 5-6. 이슈 삭제

```http
DELETE /issues/1
```

성공 응답:

```http
204 No Content
```

해당 id의 이슈가 존재하지 않는 경우:

```http
404 Not Found
```

---

### 5-7. 이슈 요약 조회

```http
GET /issues/summary
```

성공 응답:

```http
200 OK
```

```json
{
  "todo": 3,
  "doing": 2,
  "done": 1,
  "highOpen": 2,
  "total": 6
}
```

각 필드의 의미는 다음과 같습니다.

| 필드       | 의미                                         |
| -------- | ------------------------------------------ |
| todo     | status가 `TODO`인 이슈 수                       |
| doing    | status가 `DOING`인 이슈 수                      |
| done     | status가 `DONE`인 이슈 수                       |
| highOpen | priority가 `HIGH`이고 status가 `DONE`이 아닌 이슈 수 |
| total    | 전체 이슈 수                                    |

## 6. 구현한 기능

* SQLite 기반 이슈 데이터 저장
* 서버 재시작 후 데이터 유지
* 이슈 생성 기능
* 이슈 목록 조회 기능
* status, priority, assignee 기준 필터링
* priority 기준 정렬
* 이슈 단건 조회 기능
* 이슈 상태 변경 기능
* 이슈 담당자 변경 기능
* 이슈 삭제 기능
* 이슈 요약 조회 기능
* title, assignee 입력값 검증
* priority, status 값 검증
* 존재하지 않는 id 요청 시 `404 Not Found` 반환
* 생성 성공 시 `201 Created` 반환
* 삭제 성공 시 `204 No Content` 반환
* 잘못된 요청값에 대해 `400 Bad Request` 반환
* Swagger / OpenAPI를 통한 API 테스트 지원

## 7. 미구현 또는 아쉬운 부분

현재 과제 요구사항에 포함된 필수 API와 검증 로직은 구현했습니다.

다만 인증/인가 기능 및 검색어 기반 제목/설명 검색 기능은 구현하지 않았습니다.

추후 개선한다면 다음 기능을 추가할 수 있습니다.

* 사용자 인증 및 권한 기반 API 접근 제어
* 목록 조회 페이지네이션
* 제목 또는 설명 기반 키워드 검색
* 공통 에러 응답 DTO 추가
* 단위 테스트 또는 통합 테스트 추가
