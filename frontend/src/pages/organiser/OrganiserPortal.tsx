import { Link } from "react-router";
import HeroImage from "../../assets/OrganiserPortal.jpg";

export function OrganiserPortal() {
    return (
        <div className="relative min-h-screen flex items-center justify-center overflow-hidden bg-[#0a0c0b] font-black italic uppercase">

            <div className="absolute inset-0 z-0 opacity-20 grayscale contrast-150"
                 style={{ backgroundImage: `url(${HeroImage})`, backgroundSize: 'cover', backgroundPosition: 'center' }} />

            <div className="absolute inset-0 z-10 bg-[radial-gradient(circle_at_center,transparent_0%,#0a0c0b_100%)] opacity-90" />

            <div className="absolute inset-0 z-15 pointer-events-none opacity-40">
                <div className="w-full h-px bg-accent shadow-[0_0_15px_rgba(166,124,82,0.8)] animate-[scan_4s_linear_infinite]" />
            </div>

            <div className="relative z-20 text-center space-y-16 max-w-3xl px-6">
                <div className="space-y-6">
                    <div className="flex items-center justify-center gap-4">
                        <div className="h-px w-12 bg-accent/50" />
                        <h2 className="text-accent text-[10px] tracking-[0.6em] animate-pulse">
                            // Authorized_Personnel_Only
                        </h2>
                        <div className="h-px w-12 bg-accent/50" />
                    </div>

                    <div className="relative inline-block">
                        <h1 className="text-6xl md:text-9xl font-black text-white tracking-tighter leading-none relative z-10 drop-shadow-2xl">
                            COMMAND <br />
                            <span className="text-transparent bg-clip-text bg-linear-to-r from-white/10 to-white/40 border-white/10">PORTAL</span>
                        </h1>
                        <div className="absolute -top-1 -left-1 text-white/5 blur-[2px] select-none text-6xl md:text-9xl tracking-tighter">COMMAND</div>
                    </div>

                    <p className="text-white/60 text-[10px] tracking-[0.4em] font-bold max-w-sm mx-auto leading-relaxed not-italic">
                        MISSION_CONTROL_CENTER // ACCESS_RESTRICTED_TO_ORGANISER_UNITS
                    </p>
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-8 w-full max-w-2xl mx-auto">
                    <Link to="/login?role=organiser" className="group">
                        <button className="w-full py-6 bg-white/5 border border-white/20 text-white font-black text-sm tracking-[0.2em] hover:bg-white hover:text-dark transition-all duration-500 cursor-pointer relative overflow-hidden group shadow-xl">
                            <span className="relative z-10">[ SIGN_IN ]</span>
                            <div className="absolute inset-0 bg-white -translate-y-full group-hover:translate-y-0 transition-transform duration-300" />
                        </button>
                    </Link>

                    <Link to="/register?role=organiser" className="group">
                        <button className="w-full py-6 bg-accent/10 border border-accent/40 text-accent font-black text-sm tracking-[0.2em] hover:bg-accent hover:text-dark transition-all duration-500 cursor-pointer shadow-[0_0_40px_rgba(166,124,82,0.2)] active:scale-95">
                            [ REGISTER_AS_ORGANISER ]
                        </button>
                    </Link>
                </div>

                <Link to="/" className="inline-flex items-center gap-3 text-[9px] text-white/40 hover:text-accent transition-all uppercase font-black tracking-[0.4em] group">
                    <span className="group-hover:-translate-x-2 transition-transform">←</span>
                    RETURN_TO_BASE_OPERATIONS
                </Link>
            </div>
        </div>
    );
}

export default OrganiserPortal;