# SWOMS Architecture TODO

## Critical

- Harden refresh-token storage.
  Store only a salted hash of refresh tokens, not the raw token value. Treat refresh tokens like passwords because database exposure currently gives an attacker usable bearer credentials.

- Add refresh-token family/reuse detection.
  When a rotated refresh token is reused, revoke the entire token family for that user/session and require login. The current rotation flow revokes a single token but does not detect replay.

- Validate warehouse existence before stock adjustment or order creation.
  `AdjustStockAsync` can create stock for any `WarehouseId`, and `CreateOrderAsync` trusts the supplied warehouse id. Add warehouse repository checks or domain/application validation.

- Add EF Core migrations and database update workflow.
  The model is configured but there are no migrations. Add an initial migration and document production migration execution separately from app startup.

- Replace domain-generated order numbers with an application/infrastructure sequence.
  `Order` currently uses `DateTime.UtcNow` plus `Random.Shared`; collisions are possible and time generation leaks infrastructure concerns into the domain. Use a sequence/table, database-generated value, or dedicated order-number service.

## Important

- Prefer specification/query objects or typed includes over string-based repository includes.
  The generic repository currently accepts `params string[] includes`, which is fragile and hard to refactor. Preserve the repository abstraction but introduce typed query/specification support for aggregate reads.

- Add read pagination and no-tracking queries.
  `ListAsync` returns every row and tracks entities by default. Add paged query models and `AsNoTracking` for read-only paths to avoid memory and performance issues.

- Revisit repository abstraction boundaries.
  The generic repository is correctly registered and usable, but feature services are starting to express query details directly. Consider aggregate-specific repositories only where business queries become complex.

- Add validation for uniqueness before database exceptions.
  Product SKU, warehouse code, user email, and order number have unique indexes, but not all application services perform friendly pre-checks. Add application-level checks and translate unique constraint failures.

- Improve exception handling consistency.
  Add `traceId`/request id to all `ProblemDetails`, set `Content-Type` explicitly, and handle `DbUpdateException` and `SecurityTokenException`.

- Avoid logging expected client errors as warnings by default.
  Validation, 404, and domain rule failures are normal client outcomes. Log them at Information or Debug to keep production warning logs meaningful.

- Add request correlation and structured Serilog enrichers.
  Add correlation id/request id enrichment and consider environment, application name, and user id enrichment with care not to log secrets or PII.

- Review JWT option validation.
  The API validates secret length, but issuer, audience, token lifetimes, and refresh token lifetime should also be validated with options validation at startup.

- Add authorization policies/roles.
  Authenticated access is enforced for product, warehouse, and order controllers, but no role or policy separation exists for inventory adjustment, order creation, and admin actions.

- Move refresh token transport to secure cookies for browser clients.
  If the API is consumed by browsers, return refresh tokens in HttpOnly, Secure, SameSite cookies rather than response bodies.

- Add rate limiting and lockout for authentication endpoints.
  Login, register, and refresh endpoints should be protected against brute force and token-stuffing attacks.

- Add tests around core workflows.
  Cover auth registration/login/refresh, order creation with stock reservation, insufficient stock, stock adjustment, exception mapping, and EF configuration.

- Add Docker health checks and startup dependency handling.
  `depends_on` does not wait for SQL Server readiness. Add SQL health checks or retry handling for migrations/runtime connectivity.

## Nice to Have

- Add API versioning.
  Introduce URL or header-based API versioning before external clients depend on the current contract.

- Add OpenAPI operation metadata.
  Add XML comments, operation summaries, response examples, and standardized error responses to make Swagger more useful.

- Add created resource endpoints.
  `OrdersController` and `WarehousesController` create resources but do not expose `GET /{id}` endpoints for their `CreatedAtAction` targets.

- Add centralized constants for policy names, claims, and configuration sections.
  This reduces string duplication in controllers, auth setup, and security services.

- Add audit/event hooks.
  Domain events or application events would make inventory movement, order status changes, and audit trails easier to extend without bloating services.

- Add soft delete or lifecycle states where business needs require it.
  Products have `IsActive`, but other entities do not yet have a consistent lifecycle strategy.

- Add observability endpoints.
  Expand `/health` with database checks and add readiness/liveness separation for container orchestration.

- Add analyzers and formatting enforcement.
  Consider `Directory.Build.props`, nullable warnings as errors, code analysis, and formatting checks in CI.

- Add CI pipeline.
  Run restore, build, tests, vulnerability scan, and Docker build validation on every pull request.

- Add seed data for local development.
  Provide deterministic sample warehouses/products via an explicit development-only seeding path.
