# Contact WhatsApp API

A serverless contact form API that sends messages via WhatsApp using Twilio.

## Deployment to Vercel

### 1. Install Vercel CLI
```bash
npm i -g vercel
```

### 2. Login to Vercel
```bash
vercel login
```

### 3. Deploy
```bash
vercel
```

### 4. Set Environment Variables
In your Vercel dashboard, go to your project settings and add these environment variables:

- `TWILIO_ACCOUNT_SID`: Your Twilio Account SID
- `TWILIO_AUTH_TOKEN`: Your Twilio Auth Token  
- `TWILIO_FROM_NUMBER`: whatsapp:+14155238886
- `TWILIO_TO_NUMBER`: whatsapp:+919045977707

### 5. API Usage

**Endpoint:** `POST /api/contact`

**Request Body:**
```json
{
  "name": "John Doe",
  "email": "john@example.com", 
  "subject": "Test Subject",
  "message": "Test message"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Form submitted and WhatsApp sent successfully.",
  "messageSid": "SM...",
  "status": "queued"
}
```

## Local Development

1. Install dependencies: `npm install`
2. Copy `.env.example` to `.env.local` and fill in your values
3. Run: `vercel dev`
4. Test at: `http://localhost:3000/api/contact`