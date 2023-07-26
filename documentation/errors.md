# Errors

Errors described in the following section follows the RFC 7807 recommandations https://www.rfc-editor.org/rfc/rfc7807

## Bearer

| Property | Value |
| ------ | ----------- |
| **Error Code**   | bearer |
| **Http Code** | 410 |
| **Title**    | Challenge already completed |
| **Root Causes**    | - Challenge was already completed, possible request duplication |
| **Related Endpoints**    | - /auth/login/2fa/otp |

### Resolutions

- If a new token is needed, make a new challenge request and attempt it successfully

## Bunny

| Property | Value |
| ------ | ----------- |
| **Error Code**   | bunny |
| **Http Code** | 429 |
| **Title**    | Max attempts reached |
| **Root Causes**    | - Too many unsuccessfull tries to a challenge (default threshold : 3) |
| **Related Endpoints**    | - /auth/login/2fa/otp |

### Resolutions

- Make a new challenge request, and attempt this one

## Crowbar

| Property | Value |
| ------ | ----------- |
| **Error Code**   | crowbar |
| **Http Code** | 404 |
| **Title**    | Card not found |
| **Root Causes**    | - The requested card number was not found |
| **Related Endpoints**    | - /auth/login/2fa/otp |

### Resolutions

- Ensure the card number is correct
- Check its presence in database

## Flash

| Property | Value |
| ------ | ----------- |
| **Error Code**   | flash |
| **Http Code** | 404 |
| **Title**    | Account not found |
| **Root Causes**    | - The requested account Id is not registered in database |
| **Related Endpoints**    | - /cards/owner/{ownerId} |

### Resolutions

- Verify the account id
- Check account presence in database

## Ghost

| Property | Value |
| ------ | ----------- |
| **Error Code**   | glowfish |
| **Http Code** | 500 |
| **Title**    | An unknown exception occured |
| **Root Causes**    | - Possibly any exception that was not planned |
| **Related Endpoints**    | - all |

### Resolutions

- Check the service logs inside portainer
- Run the service in debug

## Glowfish

| Property | Value |
| ------ | ----------- |
| **Error Code**   | glowfish |
| **Http Code** | 409 |
| **Title**    | Account already exists |
| **Root Causes**    | - This exception can occur if the email provided in the RegisterDTO is already in the account database |
| **Related Endpoints**    | - /auth/register |

### Resolutions

- Change the email used in the RegisterDTO
- Remove the account previously created with this email manually in the Mongo database

## Jellyfish

| Property | Value |
| ------ | ----------- |
| **Error Code**   | jellyfish |
| **Http Code** | 404 |
| **Title**    | Challenge not found |
| **Root Causes**    | - No challenge exists with provided Id<br/>- No challenge request was previously made |
| **Related Endpoints**    | - /auth/login/2fa/otp |

### Resolutions

- Verify the challenge Id, it must match the challenge id received when first requesting for a challenge

## Penniless

| Property | Value |
| ------ | ----------- |
| **Error Code**   | penniless |
| **Http Code** | 402 |
| **Title**    | Insufficient funds |
| **Root Causes**    | - Insufficient funds to proceed a transaction |
| **Related Endpoints**    | - /transactions/request |

### Resolutions

- Find some money

## Sparkle

| Property | Value |
| ------ | ----------- |
| **Error Code**   | sparkle |
| **Http Code** | 401 |
| **Title**    | Invalid credentials |
| **Root Causes**    | - This exception can occur when the provided login/password combination is invalid |
| **Related Endpoints**    | - /auth/login |

### Resolutions

- Verify credentials
- Recreate an account (currently no password reset available)
