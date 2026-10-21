# IW5 Project

Some parts were inspired or taken from the CookBook example project (e.g., .gitignore)

## User Deletion
- **User deletion**: Only administrators can delete users (`DELETE /api/user/{id}`).
- **Ownership transfer**: When a user is deleted, all their existing records (places, items, and shopping lists) are automatically reassigned to the administrator performing the deletion. This prevents orphaned records and avoids losing household inventory.
