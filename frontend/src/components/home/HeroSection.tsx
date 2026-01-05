import HeroImage from "../../assets/Herosection.jpg"
import {Link} from "react-router";

export function HeroSection() {
    return (
        <div className="relative min-h-screen flex items-center justify-start overflow-hidden bg-dark]">
            <div
                className="absolute inset-0 z-0 scale-110 md:scale-105"
                style={{
                    backgroundImage: `url(${HeroImage})`,
                    backgroundSize: 'cover',
                    backgroundPosition: '70% center',
                }}
            />
            <div className="absolute inset-0 z-10 bg-linear-to-b from-dark/80 via-dark/60 to-dark/90 md:bg-linear-to-r md:from-dark md:via-dark/40 md:to-transparent"></div>

            <div className="relative z-20 container mx-auto px-6 sm:px-12 lg:px-16">
                <div className="max-w-4xl">
                    <h1 className="text-white text-4xl sm:text-6xl md:text-7xl lg:text-8xl font-black uppercase tracking-tighter leading-[0.9] mb-6">
                        Dominate <br />
                        <span className="text-transparent bg-clip-text bg-linear-to-r from-accent to-fire]">
                            The Obstacles.
                        </span>
                    </h1>
                    <p className="text-light/90 text-base md:text-xl max-w-lg mb-10 font-medium leading-relaxed border-l-4 border-secondary pl-4 md:pl-6">
                        Your central platform for OCR. Find races, register in seconds, and conquer the trail.
                    </p>
                    <div className="flex flex-col sm:flex-row gap-4 md:gap-6">
                        <Link to={"/races"}>
                        <button className="w-full sm:w-auto group relative px-8 md:px-10 py-4 md:py-5 bg-accent text-dark font-black uppercase tracking-tighter transition-all hover:-translate-y-1 hover:shadow-[0_8px_0_0_rgba(166,124,82,1)] active:translate-y-0 active:shadow-none cursor-pointer text-center">
                            Browse Races
                        </button>
                        </Link>
                        <button className="w-full sm:w-auto px-8 md:px-10 py-4 md:py-5 bg-white/5 backdrop-blur-md border border-white/20 text-white font-bold uppercase tracking-tighter hover:bg-white/10 transition-all cursor-pointer text-center">
                            For Organizers
                        </button>
                    </div>

                </div>
            </div>
        </div>
    )
}

export default HeroSection;
