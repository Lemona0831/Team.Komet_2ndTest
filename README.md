# Team.Komet 2nd Test

개발팀 2차 실기 평가 제출용 GitHub Repository입니다.

본 저장소에는 1번 과제와 2번 과제의 프로젝트 코드, 각 과제별 README, 그리고 수정 보고서가 포함되어 있습니다.

## 1. 제출물 구성

| 구분           | 폴더 / 파일                              | 설명                              |
| ------------ | ------------------------------------ | ------------------------------- |
| 1번 과제        | `task1-issue-tracker-api`            | Issue Tracker REST API 구현 과제    |
| 2번 과제        | `task2-buggy-user-api`               | Buggy User Management API 수정 과제 |
| 2번 과제 수정 보고서 | `task2-buggy-user-api/FIX_REPORT.md` | 2번 과제에서 발견한 문제와 수정 내용을 정리한 보고서  |

## 2. 과제별 개요

### 2-1. Task 1 - Issue Tracker REST API

`task1-issue-tracker-api` 폴더에 위치한 프로젝트입니다.

C#과 ASP.NET Core를 사용하여 Issue Tracker REST API를 구현했습니다.

주요 기능은 다음과 같습니다.

* 이슈 생성
* 이슈 목록 조회
* 이슈 단건 조회
* 이슈 상태 변경
* 이슈 담당자 변경
* 이슈 삭제
* 이슈 요약 조회
* SQLite 기반 데이터 저장
* Swagger를 통한 API 테스트 지원

자세한 실행 방법과 API 예시는 아래 파일을 참고하면 됩니다.

* `task1-issue-tracker-api/README.md`

### 2-2. Task 2 - Buggy User Management API

`task2-buggy-user-api` 폴더에 위치한 프로젝트입니다.

제공된 ASP.NET Core 스타터 프로젝트를 기반으로, 의도적으로 포함되어 있던 사용자 관리 API의 버그와 설계 문제를 수정했습니다.

주요 수정 내용은 다음과 같습니다.

* HTTP 상태 코드 처리 수정
* 존재하지 않는 사용자 요청 처리
* 사용자 삭제 로직 수정
* 삭제된 사용자 조회 제외
* 사용자 생성 입력값 검증
* role 값 검증
* 이메일 중복 검사
* password 응답 노출 방지
* UserResponse 매핑 오류 수정

자세한 실행 방법과 API 예시는 아래 파일을 참고하면 됩니다.

* `task2-buggy-user-api/README.md`

수정한 문제와 해결 내용은 아래 보고서에 정리했습니다.

* `task2-buggy-user-api/FIX_REPORT.md`

## 3. 실행 환경

두 과제 모두 아래 환경을 기준으로 작성했습니다.

* C#
* ASP.NET Core
* .NET 10 SDK
* JSON 요청/응답

1번 과제는 SQLite와 Entity Framework Core를 사용합니다.

2번 과제는 과제 조건에 따라 In-Memory List 저장 방식을 사용합니다.

## 4. 실행 방법 요약

### 4-1. 1번 과제 실행

저장소 루트에서 아래 폴더로 이동합니다.

```
cd task1-issue-tracker-api
```

CLI로 실행하는 경우 다음 명령어를 사용합니다.

```
dotnet restore
dotnet run --project "Issue Tracker/Issue Tracker.csproj"
```

또는 Visual Studio에서 `Issue Tracker.sln` 파일을 열어 실행할 수 있습니다.

실행 후 콘솔에 출력되는 주소 뒤에 `/swagger`를 붙여 API를 테스트할 수 있습니다.

예시:

```
https://localhost:7056/swagger
```

포트 번호는 실행 환경에 따라 달라질 수 있습니다.

### 4-2. 2번 과제 실행

저장소 루트에서 아래 폴더로 이동합니다.

```
cd task2-buggy-user-api
```

CLI로 실행하는 경우 다음 명령어를 사용합니다.

```
dotnet restore
dotnet run
```

실행 후 콘솔에 출력되는 주소를 기준으로 Postman 또는 curl을 사용해 API를 테스트할 수 있습니다.

예시:

```
http://localhost:60273/users
```

포트 번호는 실행 환경에 따라 달라질 수 있습니다.

## 5. 문서 위치

| 문서           | 위치                                   |
| ------------ | ------------------------------------ |
| 1번 과제 README | `task1-issue-tracker-api/README.md`  |
| 2번 과제 README | `task2-buggy-user-api/README.md`     |
| 2번 과제 수정 보고서 | `task2-buggy-user-api/FIX_REPORT.md` |

## 6. 참고 사항

* 1번 과제는 SQLite DB 파일을 자동 생성하도록 구현했습니다.
* 2번 과제는 과제 조건상 메모리 저장 방식을 사용했습니다.
* 각 과제의 상세 실행 방법, API 목록, 요청/응답 예시는 각 과제 폴더의 README에 따로 정리했습니다.
