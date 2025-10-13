# Pull Request

## 📋 Description
<!-- Provide a clear and concise description of what this PR does -->



## 🎯 Type of Change
<!-- Mark the relevant option with an [x] -->

- [ ] 🐛 Bug fix (non-breaking change which fixes an issue)
- [ ] ✨ New feature (non-breaking change which adds functionality)
- [ ] 💥 Breaking change (fix or feature that would cause existing functionality to not work as expected)
- [ ] 📝 Documentation update
- [ ] ♻️ Code refactoring (no functional changes)
- [ ] ⚡ Performance improvement
- [ ] ✅ Test addition or improvement
- [ ] 🏗️ Infrastructure/Build changes

## 🏗️ Architecture Layer Impact
<!-- Mark all layers affected by this PR -->

- [ ] **API Layer** - Controllers, Filters, HTTP concerns
- [ ] **Application Layer** - Services, DTOs, Validators
- [ ] **Domain Layer** - Entities, Enums, Value Objects
- [ ] **Infrastructure Layer** - Repositories, External integrations
- [ ] **Shared Layer** - Cross-cutting concerns
- [ ] **Tests** - Unit tests or Integration tests

## 🧪 Testing
<!-- Describe the tests you've added or how you've tested this change -->

### Unit Tests
- [ ] Added/Updated unit tests
- [ ] All unit tests pass locally

### Integration Tests
- [ ] Added/Updated integration tests
- [ ] All integration tests pass locally

### Manual Testing
<!-- Describe any manual testing performed -->



## ✅ Checklist
<!-- Ensure all items are completed before requesting review -->

### Code Quality
- [ ] My code follows the Clean Architecture principles
- [ ] I have performed a self-review of my code
- [ ] My code follows the SOLID principles
- [ ] I have made minimal modifications (surgical changes only)
- [ ] I have commented my code where necessary (explaining "why", not "what")
- [ ] Variable and method names are descriptive and follow naming conventions

### Documentation
- [ ] I have updated relevant documentation
- [ ] I have updated XML comments for public APIs
- [ ] I have updated the CHANGELOG.md if applicable

### Testing
- [ ] I have added tests that prove my fix/feature works
- [ ] New and existing unit tests pass locally
- [ ] New and existing integration tests pass locally
- [ ] I have verified my changes don't break existing functionality

### Dependencies
- [ ] Dependencies are added only to the appropriate layer
- [ ] No unnecessary library updates or additions
- [ ] Layer dependencies follow Clean Architecture rules:
  - Domain has no dependencies on other layers
  - Infrastructure and Application reference Domain
  - API references Application

### Validation
- [ ] I have used FluentValidation for DTO validation where applicable
- [ ] Error messages are centralized in `ErrorConstants.cs`
- [ ] MongoDB ObjectId validation uses `MongoDbHelper` utilities

## 📎 Related Issues
<!-- Link to related issues using #issue_number -->

Closes #
Related to #

## 📸 Screenshots/Evidence
<!-- If applicable, add screenshots or logs to demonstrate the changes -->



## 🔍 Additional Context
<!-- Add any other context about the PR here -->



---
### Reviewer Notes
<!-- Space for reviewer comments and feedback -->

