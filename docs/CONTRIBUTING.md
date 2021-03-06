# Contributing guidelines
These are the contributing guidelines for Windows 11 ISO patcher. Failure to comply with these contributing guidelines may result in the PR being closed.

- Use descriptive PR/Commit names and descriptions.

  **Acceptable:**
  - Add appraiserres.dll patch
  - Remove unused variables from MainWindow.cs

  **Unacceptable:**
  - Add files via upload
  - Remove files

- Keep your PR/commit names short.
  
  Try keeping them below 50 characters. If you need to give that much level of detail, try splitting them into smaller commits or using the description box.
  
- Checking for check failures

  If any checks failed, check their logs to see what changes you made that caused it to fail.
  
- Testing

If you have tested your contributions, include the test results in the PR Description. A testing note may look like this:

Build     | Success? | Notes                                                       |
----------|----------|-------------------------------------------------------------|
22000.168 | Yes      | Works correctly in testing on a Secure Boot incompatible PC |
22000.51  | No       | Failure at step 3                                           |
